using System;
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
        Command<TakeHourlySnapshot>(_ => SaveSnapshot(_state));
        Recover<SnapshotOffer>(offer => { _state = (NormalizedReadingPersistenceState)offer.Snapshot; });

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

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<HotStorageActor>(deviceId);
    }
}