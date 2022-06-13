using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

public class DeviceActor : ReceiveActor
{
    private readonly Guid _deviceId;
    private IActorRef _normalizationActor;
    private IActorRef _storageActor;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
        CreateChildren();
        Receive<MeterReadingReceived>(_normalizationActor.Forward);
        Receive<NormalizedMeterReading>(DistributeNormalizedReadings);
    }

    private void DistributeNormalizedReadings(NormalizedMeterReading reading)
    {
        _storageActor.Tell(reading);
    }

    private void CreateChildren()
    {
        var normalizationProps = ValueNormalizationActor.CreateProps();
        _normalizationActor = Context.ActorOf(normalizationProps, "value-normalization");
        var storageProps = HotStorageActor.CreateProps(_deviceId);
        _storageActor = Context.ActorOf(storageProps, "value-storage");
    }

    
    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}