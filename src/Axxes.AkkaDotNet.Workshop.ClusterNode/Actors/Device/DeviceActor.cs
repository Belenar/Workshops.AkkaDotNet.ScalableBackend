using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

internal class DeviceActor : ReceiveActor
{
    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Receive<MeterReadingReceived>(HandleMeterReading);
    }

    private void HandleMeterReading(MeterReadingReceived obj)
    {
        Context.GetLogger().Debug($"Meter reading received: {obj.MeterReading}");
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}

