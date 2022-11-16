using System;
using Akka.Routing;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages;

public record ConnectDevice(Guid DeviceId) : IConsistentHashable
{
    public object ConsistentHashKey => DeviceId;
}