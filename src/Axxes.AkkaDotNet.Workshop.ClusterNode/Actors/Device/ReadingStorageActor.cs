using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaDotNet.Workshop.ClusterNode.State;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

internal class ReadingStorageActor : ReceivePersistentActor
{
    private const string DbWriterActor = "historic-storage";
    private readonly Guid _deviceId;
    public override string PersistenceId => $"device-storage-{_deviceId}";

    private NormalizedReadingPersistenceState _state = new();

    public ReadingStorageActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Context.ActorOf(ReadingDbWriterActor.CreateProps(_deviceId), DbWriterActor);

        Recover<SnapshotOffer>(HandleSnapshotOffer);

        Command<NormalizedMeterReading>(
            msg => Persist(msg, ApplyNormalizedReading));
        Recover<NormalizedMeterReading>(ApplyNormalizedReading);

        Command<WrittenReadingsToDatabase>(
            msg => Persist(msg, ApplyWrittenToDatabase));
        Recover<WrittenReadingsToDatabase>(ApplyWrittenToDatabase);

        Command<TakeHourlySnapshot>(_ => SaveSnapshot(_state));
        Command<WriteHourlyReadings>(_ => WriteReadingsToColdStorage());
        Command<RequestLastNormalizedReadings>(HandleRequestLastNormalizedReading);

        // Gets rid of dead letters
        Command<SaveSnapshotFailure>(_ => { });
        Command<SaveSnapshotSuccess>(_ => { });

        ScheduleSnapshots();
    }

    private void ApplyNormalizedReading(NormalizedMeterReading reading)
    {
        _state.Add(reading);
    }

    private void ApplyWrittenToDatabase(WrittenReadingsToDatabase message)
    {
        _state.SetSavedUntil(message.WrittenToDate);
        _state.Truncate();
    }

    private void WriteReadingsToColdStorage()
    {
        var message = new WriteReadingsToDatabase(ImmutableArray.Create(_state.GetUnsavedItems()));
        Context.Child(DbWriterActor).Tell(message);
    }

    private void HandleRequestLastNormalizedReading(RequestLastNormalizedReadings message)
    {
        var lastReadings = _state.GetLastReadings(message.NumberOfReadings);
        var response = new ReturnLastNormalizedReadings(ImmutableArray.Create(lastReadings));
        Sender.Tell(response);
    }

    private void HandleSnapshotOffer(SnapshotOffer offer)
    {
        if (offer.Snapshot is NormalizedReadingPersistenceState state)
            _state = state;
    }

    private void ScheduleSnapshots()
    {
        var offsetSeconds = new Random().Next(3600);
        var interval = new TimeSpan(0, 1, 0);

        // Schedule DB writes
        Context.System.Scheduler.ScheduleTellRepeatedly(
            TimeSpan.FromSeconds(offsetSeconds),
            interval,
            Self,
            new WriteHourlyReadings(),
            Self);

        // Schedule snapshots 60 seconds later
        Context.System.Scheduler.ScheduleTellRepeatedly(
            TimeSpan.FromSeconds(60 + offsetSeconds),
            interval,
            Self,
            new TakeHourlySnapshot(),
            Self);
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<ReadingStorageActor>(deviceId);
    }
}