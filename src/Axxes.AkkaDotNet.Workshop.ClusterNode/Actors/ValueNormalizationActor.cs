using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class ValueNormalizationActor : ReceiveActor, IWithUnboundedStash
{
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
        Receive<ReturnLastNormalizedReadings>(SetReferenceValues);
        Receive<MeterReadingReceived>(_ => Stash.Stash());
    }

    private void Initialized()
    {
        Receive<MeterReadingReceived>(HandleMeterReadingReceived);
    }

    private void SetReferenceValues(ReturnLastNormalizedReadings message)
    {
        if (message.Readings.Any())
        {
            var reference = message.Readings.First();
            _helper.SetReferenceReading(reference.Timestamp, reference.MeterReading);
        }
        Become(Initialized);
        Stash.UnstashAll();
    }

    private void HandleMeterReadingReceived(MeterReadingReceived message)
    {
        var normalizedReadings = _helper.GetNormalizedReadingsUntil(message.TimestampUtc, message.MeterReading);

        foreach (var normalizedMeterReading in normalizedReadings)
        {
            Context.Parent.Tell(normalizedMeterReading);
        }
    }

    public static Props CreateProps()
    {
        return Props.Create<ValueNormalizationActor>();
    }

    public IStash Stash { get; set; }
}