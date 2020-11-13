using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.Actors
{
    class DeviceActorProxy : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        private readonly Guid _deviceId;
        private readonly IActorRef _devicesRouter;
        private IActorRef _deviceActor;

        public DeviceActorProxy(Guid deviceId, IActorRef devicesRouter)
        {
            _deviceId = deviceId;
            _devicesRouter = devicesRouter;
            Become(Started);
            
        }

        protected override void PreStart()
        {
            var request = new ConnectDevice(_deviceId);
            _devicesRouter.Tell(request);
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

        public static Props CreateProps(Guid deviceId, IActorRef devicesRouter)
        {
            return Props.Create<DeviceActorProxy>(deviceId, devicesRouter);
        }
    }
}
