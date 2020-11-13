using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device
{
    class DeviceActor : ReceiveActor
    {
        private readonly Guid _deviceId;
        private IActorRef _normalizationActor;

        public DeviceActor(Guid deviceId)
        {
            _deviceId = deviceId;
            CreateChildActors();
            Receive<MeterReadingReceived>(HandleMeterReadingReceived);
            Receive<NormalizedMeterReading>(HandleNormalizedMeterReading);
        }

        private void CreateChildActors()
        {
            var normalizationProps = ValueNormalizationActor.CreateProps();
            _normalizationActor = Context.ActorOf(normalizationProps, "value-normalization");
        }

        private void HandleMeterReadingReceived(MeterReadingReceived msg)
        {
            _normalizationActor.Forward(msg);
        }

        private void HandleNormalizedMeterReading(NormalizedMeterReading msg)
        {
            Context.GetLogger().Info($"NormalizedMeterReading handled in DeviceActor {_deviceId}.");
        }

        public static Props CreateProps(Guid deviceId)
        {
            return Props.Create<DeviceActor>(deviceId);
        }
    }
}
