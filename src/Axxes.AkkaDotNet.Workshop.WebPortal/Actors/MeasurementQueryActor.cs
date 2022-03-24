using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Actors
{
    public class MeasurementQueryActor : ReceiveActor, IWithUnboundedStash
    {
        private readonly IActorRef _allDevicesActor;
        private readonly Guid _deviceId;
        private IActorRef _deviceActor;

        public MeasurementQueryActor(IActorRef allDevicesActor, Guid deviceId)
        {
            _allDevicesActor = allDevicesActor;
            _deviceId = deviceId;
            
            Become(Started);
        }

        private void Started()
        {
            Receive<QueryMeasurementsData>(_ => Stash.Stash());
            Receive<DeviceConnected>(HandleDeviceConnected);
        }

        private void Initialized()
        {
            Receive<QueryMeasurementsData>(msg =>
            {
                _deviceActor.Forward(msg);
                Self.Tell(PoisonPill.Instance);
            });
        }

        private void HandleDeviceConnected(DeviceConnected message)
        {
            _deviceActor = message.DeviceActor;
            Become(Initialized);
            Stash.UnstashAll();
        }

        protected override void PreStart()
        {
            _allDevicesActor.Tell(new ConnectDevice(_deviceId));
        }

        protected override void PostStop()
        {
            Context.GetLogger().Info("Query actor terminated.");
        }

        public static Props CreateProps(IActorRef allDevicesActor, Guid deviceId)
        {
            return Props.Create<MeasurementQueryActor>(allDevicesActor, deviceId);
        }

        public IStash Stash { get; set; }
    }
}
