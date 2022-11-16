using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using System.Linq;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

class ValueNormalizationActor : ReceiveActor, IWithUnboundedStash
{
    public IStash Stash { get; set; }

    private readonly ValueNormalizationHelper _helper = new();

    public ValueNormalizationActor()
    {
        Become(Started);
    }

    protected override void PreStart()
    {
        Context.Parent.Tell(new RequestLastNormalizedReadings(1));
    }

    private void Started()
    {
        Receive<ReturnLastNormalizedReadings>(HandleReturnLastNormalizedReadings);
        Receive<MeterReadingReceived>(_ => Stash.Stash());
    }

    private void Initialized()
    {
        Receive<MeterReadingReceived>(HandleMeterReadingReceived);
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

    private void HandleMeterReadingReceived(MeterReadingReceived message)
    {
        var newNormalizedReadings =
            _helper.GetNormalizedReadingsUntil(message.TimestampUtc, message.MeterReading);

        foreach (var normalizedMeterReading in newNormalizedReadings)
        {
            Context.Parent.Tell(normalizedMeterReading);
        }
    }

    public static Props CreateProps()
    {
        return Props.Create<ValueNormalizationActor>();
    }
}