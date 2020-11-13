using System;
using System.Linq;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device
{
    class ValueNormalizationActor : ReceiveActor, IWithUnboundedStash
    {
        public IStash Stash { get; set; }

        private readonly IActorRef _persistenceActor;
        private readonly ValueNormalizationHelper _helper = new ValueNormalizationHelper();

        public ValueNormalizationActor(IActorRef persistenceActor)
        {
            _persistenceActor = persistenceActor;
            Become(Started);
        }

        private void Started()
        {
            Receive<ReturnLastNormalizedReadings>(HandleReturnLastNormalizedReadings);
            Receive<MeterReadingReceived>(StashMeterReadingReceived);
        }

        private void Initialized()
        {
            Receive<MeterReadingReceived>(HandleMeterReadingReceived);
        }

        protected override void PreStart()
        {
            _persistenceActor.Tell(new RequestLastNormalizedReadings(1));
        }

        private void HandleReturnLastNormalizedReadings(ReturnLastNormalizedReadings msg)
        {
            if (msg.Readings.Any())
            {
                var referenceReading = msg.Readings.First();
                // Set the reference data only
                // ReSharper disable once IteratorMethodResultIsIgnored
                _helper.GetNormalizedReadingsUntil(referenceReading.Timestamp, referenceReading.MeterReading);
            }
            Become(Initialized);
            Stash.UnstashAll();
        }

        private void StashMeterReadingReceived(MeterReadingReceived message)
        {
            Stash.Stash();
        }

        private void HandleMeterReadingReceived(MeterReadingReceived message)
        {
            var newNormalizedReadings =
                _helper.GetNormalizedReadingsUntil(message.TimestampUtc, message.MeterReading);

            foreach (var normalizedMeterReading in newNormalizedReadings)
            {
                Context.Parent.Tell(normalizedMeterReading);
            }
        }

        public static Props CreateProps(IActorRef persistenceActor)
        {
            return Props.Create<ValueNormalizationActor>(persistenceActor);
        }
    }
}