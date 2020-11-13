using System;
using System.Threading;
using System.Threading.Tasks;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.System
{
    public interface IActorSystemService
    {
        void SendMeasurement(Guid deviceId, MeterReadingReceived message);
        Task StopAsync(CancellationToken cancellationToken);
    }
}