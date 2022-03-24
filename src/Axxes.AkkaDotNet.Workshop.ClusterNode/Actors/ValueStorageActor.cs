using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaDotNet.Workshop.ClusterNode.State;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class ValueStorageActor : ReceivePersistentActor
{
    private const string DbWriterName = "historic-storage";

    public override string PersistenceId { get; }

    private NormalizedReadingPersistenceState _state = new();

    public ValueStorageActor(Guid deviceId)
    {
        PersistenceId = $"value-storage-{deviceId}";
        Recover<SnapshotOffer>(RestoreSnapshot);
        Recover<NormalizedMeterReading>(msg => _state.Add(msg));
        Recover<WrittenReadingsToDatabase>(SetStateSavedUntil);
        Command<NormalizedMeterReading>(HandleNormalizedMeterReading);
        Command<TakeHourlySnapshot>(WriteHourlyData);
        Command<RequestLastNormalizedReadings>(HandleRequestLastNormalizedReadings);
        Command<WrittenReadingsToDatabase>(HandleWrittenReadingsToDatabase);

        Context.ActorOf(ReadingDbWriterActor.CreateProps(deviceId), DbWriterName);

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

    private void WriteHourlyData(TakeHourlySnapshot obj)
    {
        SaveSnapshot(_state);
        var unsavedData = _state.GetUnsavedItems();
        Context.Child(DbWriterName).Tell(new WriteReadingsToDatabase(ImmutableArray.Create(unsavedData)));
    }

    private void HandleWrittenReadingsToDatabase(WrittenReadingsToDatabase message)
    {
        Persist(message,SetStateSavedUntil);
    }

    private void SetStateSavedUntil(WrittenReadingsToDatabase message)
    {
        _state.SetSavedUntil(message.WrittenToDate);
        _state.Truncate();
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