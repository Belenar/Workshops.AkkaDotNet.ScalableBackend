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
        private IActorRef _persistenceActor;

        public DeviceActor(Guid deviceId)
        {
            _deviceId = deviceId;
            CreateChildActors();
            Receive<MeterReadingReceived>(HandleMeterReadingReceived);
            Receive<NormalizedMeterReading>(HandleNormalizedMeterReading);
        }

        private void CreateChildActors()
        {
            var persistenceProps = ReadingPersistenceActor.CreateProps(_deviceId);
            _persistenceActor = Context.ActorOf(persistenceProps, "value-peristence");
            var normalizationProps = ValueNormalizationActor.CreateProps(_persistenceActor);
            _normalizationActor = Context.ActorOf(normalizationProps, "value-normalization");
        }

        private void HandleMeterReadingReceived(MeterReadingReceived msg)
        {
            _normalizationActor.Forward(msg);
        }

        private void HandleNormalizedMeterReading(NormalizedMeterReading msg)
        {
            _persistenceActor.Tell(msg);
        }

        public static Props CreateProps(Guid deviceId)
        {
            return Props.Create<DeviceActor>(deviceId);
        }
    }
}
