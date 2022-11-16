using System;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaDotNet.Workshop.ClusterNode.State;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

internal class ReadingStorageActor : ReceivePersistentActor
{
    private readonly Guid _deviceId;
    public override string PersistenceId => $"device-storage-{_deviceId}";

    private NormalizedReadingPersistenceState _state = new();

    public ReadingStorageActor(Guid deviceId)
    {
        _deviceId = deviceId;

        Command<NormalizedMeterReading>(
            msg => Persist(msg, ApplyNormalizedReading));
        Recover<NormalizedMeterReading>(ApplyNormalizedReading);

        Command<TakeHourlySnapshot>(_ => SaveSnapshot(_state));

        ScheduleSnapshots();
    }



    private void ApplyNormalizedReading(NormalizedMeterReading reading)
    {
        _state.Add(reading);
    }

    private void ScheduleSnapshots()
    {
        var offsetSeconds = new Random().Next(3600);
        var offset = TimeSpan.FromSeconds(offsetSeconds);
        var interval = new TimeSpan(0, 1, 0);
        
        Context.System.Scheduler.ScheduleTellRepeatedly(
            offset,
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

