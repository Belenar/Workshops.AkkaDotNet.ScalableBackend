using System;
using Akka.Actor;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages;

public record DeviceConnected(Guid DeviceId, IActorRef DeviceActor);