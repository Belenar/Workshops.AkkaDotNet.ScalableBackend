using System;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class QueryMeasurementsData
    {
        public DateTime FromDateUtc { get; }
        public DateTime ToDateUtc { get; }

        public QueryMeasurementsData(DateTime fromDateUtc, DateTime toDateUtc)
        {
            FromDateUtc = fromDateUtc;
            ToDateUtc = toDateUtc;
        }
    }
}