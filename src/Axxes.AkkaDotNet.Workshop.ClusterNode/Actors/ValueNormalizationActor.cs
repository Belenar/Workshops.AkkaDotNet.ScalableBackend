using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class ValueNormalizationActor : ReceiveActor
{
    private readonly ValueNormalizationHelper _helper = new();

    public ValueNormalizationActor()
    {
        Receive<MeterReadingReceived>(HandleMeterReadingReceived);
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
}