using System;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaDotNet.Workshop.ClusterNode.State;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

public class HotStorageActor : ReceivePersistentActor
{
    public override string PersistenceId { get; }

    private NormalizedReadingPersistenceState _state = new();

    public HotStorageActor(Guid deviceId)
    {
        PersistenceId = $"device-storage-{deviceId}";
        ScheduleSnapshots();
        
        Command<NormalizedMeterReading>(msg => Persist(msg, HandleNewMeterReading));
        Recover<NormalizedMeterReading>(HandleNewMeterReading);
        Command<WrittenReadingsToDatabase>(msg => Persist(msg, m => _state.SetSavedUntil(m.WrittenToDate)));

        Command<TakeHourlySnapshot>(HandleSnapshot);
        Command<SaveSnapshotSuccess>(_ => { });
        Command<SaveSnapshotFailure>(_ => { });
        Recover<SnapshotOffer>(offer => { _state = (NormalizedReadingPersistenceState)offer.Snapshot; });

        Command<RequestLastNormalizedReadings>(ReturnNormalizedReadings);

        var coldStorageProps = ColdStorageActor.CreateProps(deviceId);
        Context.ActorOf(coldStorageProps, "historic-storage");
    }

    private void ScheduleSnapshots()
    {
        var minutesUntilFirst = new Random().Next(60);

        var initialDelay = TimeSpan.FromMinutes(minutesUntilFirst);

        Context.System.Scheduler.ScheduleTellRepeatedly(
            initialDelay,
            TimeSpan.FromHours(1), 
            Self,
            new TakeHourlySnapshot(),
            Self);
    }

    private void HandleNewMeterReading(NormalizedMeterReading reading)
    {
        _state.Add(reading);
    }

    private void ReturnNormalizedReadings(RequestLastNormalizedReadings request)
    {
        var lastReadings = _state.GetLastReadings(request.NumberOfReadings);
        Sender.Tell(new ReturnLastNormalizedReadings(lastReadings.ToImmutableArray()));
    }

    private void HandleSnapshot(TakeHourlySnapshot obj)
    {
        _state.Truncate();
        SaveSnapshot(_state);
        var unsavedReadings = _state.GetUnsavedItems().ToImmutableArray();
        if(unsavedReadings.Any())
            Context.Child("historic-storage").Tell(new WriteReadingsToDatabase(unsavedReadings));
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<HotStorageActor>(deviceId);
    }
}