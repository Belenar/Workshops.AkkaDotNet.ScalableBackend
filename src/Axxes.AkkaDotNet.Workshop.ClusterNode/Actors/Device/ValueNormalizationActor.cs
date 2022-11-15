using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

class ValueNormalizationActor : ReceiveActor
{
    private readonly ValueNormalizationHelper _helper = new();

    public ValueNormalizationActor()
    {
        Receive<MeterReadingReceived>(HandleMeterReadingReceived);
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