using Akka.Actor;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class DeviceConnected
    {
        public IActorRef DeviceRef { get; }

        public DeviceConnected(IActorRef deviceRef)
        {
            DeviceRef = deviceRef;
        }
    }
}