using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaDotNet.Workshop.ClusterNode.State;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class ValueStorageActor : ReceivePersistentActor
{
    public override string PersistenceId { get; }

    private NormalizedReadingPersistenceState _state = new();

    public ValueStorageActor(Guid deviceId)
    {
        PersistenceId = $"value-storage-{deviceId}";
        Recover<SnapshotOffer>(RestoreSnapshot);
        Recover<NormalizedMeterReading>(msg => _state.Add(msg));
        Command<NormalizedMeterReading>(HandleNormalizedMeterReading);
        Command<TakeHourlySnapshot>(_ => SaveSnapshot(_state));
        Command<RequestLastNormalizedReadings>(HandleRequestLastNormalizedReadings);

        Context.System.Scheduler.ScheduleTellRepeatedly(
            TimeSpan.FromSeconds(10),
            TimeSpan.FromHours(1),
            Self,
            new TakeHourlySnapshot(),
            Self);
    }

    private void RestoreSnapshot(SnapshotOffer offer)
    {
        var snapshot = (NormalizedReadingPersistenceState)offer.Snapshot;
        _state = snapshot;
    }

    private void HandleNormalizedMeterReading(NormalizedMeterReading message)
    {
        Persist(message, msg => _state.Add(msg));
    }

    private void HandleRequestLastNormalizedReadings(RequestLastNormalizedReadings message)
    {
        var readings = _state.GetLastReadings(message.NumberOfReadings);
        Sender.Tell(new ReturnLastNormalizedReadings(ImmutableArray.Create(readings)));
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<ValueStorageActor>(deviceId);
    }
}