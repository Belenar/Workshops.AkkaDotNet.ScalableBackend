using System;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.Models
{
    public class Measurement
    {
        public DateTime TimestampUtc { get; set; }
        public decimal Reading { get; set; }
        public decimal Consumption { get; set; }
    }
}