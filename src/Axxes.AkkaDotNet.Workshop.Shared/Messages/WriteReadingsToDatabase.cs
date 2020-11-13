using System.Collections.Immutable;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class WriteReadingsToDatabase
    {
        public ImmutableArray<NormalizedMeterReading> Readings { get; }

        public WriteReadingsToDatabase(ImmutableArray<NormalizedMeterReading> readings)
        {
            Readings = readings;
        }
    }
}
