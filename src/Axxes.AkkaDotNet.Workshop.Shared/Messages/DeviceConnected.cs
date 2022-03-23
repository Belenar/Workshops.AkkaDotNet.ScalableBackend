using System;
using Akka.Actor;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages;

public class DeviceConnected
{
    public DeviceConnected(Guid deviceId, IActorRef deviceActor)
    {
        DeviceId = deviceId;
        DeviceActor = deviceActor;
    }

    public Guid DeviceId { get; }
    public IActorRef DeviceActor { get; }
}