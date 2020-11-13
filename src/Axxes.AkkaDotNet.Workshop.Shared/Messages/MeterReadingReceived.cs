using System;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class MeterReadingReceived
    {
        public Guid DeviceId { get; }
        public DateTime TimestampUtc { get; }
        public decimal MeterReading { get; }

        public MeterReadingReceived(Guid deviceId, DateTime timestampUtc, decimal meterReading)
        {
            DeviceId = deviceId;
            TimestampUtc = timestampUtc;
            MeterReading = meterReading;
        }
    }
}
