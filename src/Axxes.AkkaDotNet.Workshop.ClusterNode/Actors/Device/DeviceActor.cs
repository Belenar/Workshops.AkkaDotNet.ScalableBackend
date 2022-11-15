using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

internal class DeviceActor : ReceiveActor
{
    private const string NormalizationActorName = "value-normalization";
    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
        CreateChildren();
        Receive<MeterReadingReceived>(HandleMeterReading);
        Receive<NormalizedMeterReading>(HandleNormalizedReading);
    }

    private void CreateChildren()
    {
        var normalizationProps = ValueNormalizationActor.CreateProps();
        Context.ActorOf(normalizationProps, NormalizationActorName);
    }

    private void HandleMeterReading(MeterReadingReceived reading)
    {
        Context.Child(NormalizationActorName).Tell(reading);
    }

    private void HandleNormalizedReading(NormalizedMeterReading obj)
    {
        Context.GetLogger().Debug($"Normalized reading received: {obj.MeterReading}");
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}

