using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.MessageReader.System;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.Actors
{
    class DeviceActorProxy : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        private readonly Guid _deviceId;
        private IActorRef _deviceActor;

        public DeviceActorProxy(Guid deviceId)
        {
            _deviceId = deviceId;
            Become(Started);
            
        }

        protected override void PreStart()
        {
            var devicesActorPath = $"{SystemConstants.RemoteSystemAddress}/user/devices";
            var devicesActor = Context.ActorSelection(devicesActorPath);

            var request = new ConnectDevice(_deviceId);
            devicesActor.Tell(request);
        }

        private void Started()
        {
            Receive<DeviceConnected>(HandleDeviceConnected);
            Receive<MeterReadingReceived>(StashMeterReadingReceived);
        }

        private void Connected()
        {
            Receive<MeterReadingReceived>(HandleMeterReadingReceived);
        }

        private void StashMeterReadingReceived(MeterReadingReceived msg)
        {
            Stash.Stash();
        }

        private void HandleDeviceConnected(DeviceConnected message)
        {
            _deviceActor = message.DeviceRef;
            Become(Connected);
            Stash.UnstashAll();
        }

        private void HandleMeterReadingReceived(MeterReadingReceived message)
        {
            _deviceActor.Tell(message);
        }

        public static Props CreateProps(Guid deviceId)
        {
            return Props.Create<DeviceActorProxy>(deviceId);
        }
    }
}
