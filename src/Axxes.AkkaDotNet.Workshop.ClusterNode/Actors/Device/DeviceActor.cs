using System;
using Akka.Actor;
using Akka.Event;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

internal class DeviceActor : ReceiveActor
{
    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Context.GetLogger().Debug($"Device Actor for device {_deviceId} was created");
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}

