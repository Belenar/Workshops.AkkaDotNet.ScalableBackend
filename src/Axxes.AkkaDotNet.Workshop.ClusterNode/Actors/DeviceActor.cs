using System;
using Akka.Actor;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class DeviceActor : UntypedActor
{
    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
    }

    protected override void OnReceive(object message)
    {
        throw new NotImplementedException();
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}