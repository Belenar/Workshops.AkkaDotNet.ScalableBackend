using System;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.System
{
    public interface IActorSystemService
    {
        void SendMeasurement(Guid deviceId, MeterReadingReceived message);
    }
}