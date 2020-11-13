using System.Collections.Immutable;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class MeasurementData
    {
        public ImmutableArray<NormalizedMeterReading> Readings { get; }

        public MeasurementData(ImmutableArray<NormalizedMeterReading> readings)
        {
            Readings = readings;
        }
    }
}