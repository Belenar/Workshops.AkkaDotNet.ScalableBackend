using Akka.Actor;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages;

public record class DeviceConnected(IActorRef DeviceActor);