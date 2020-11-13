using System.Collections.Immutable;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class ReturnLastNormalizedReadings
    {
        public ImmutableArray<NormalizedMeterReading> Readings { get; }

        public ReturnLastNormalizedReadings(ImmutableArray<NormalizedMeterReading> readings)
        {
            Readings = readings;
        }
    }
}