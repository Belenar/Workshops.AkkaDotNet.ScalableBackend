using System;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.System
{
    public class ActorSystemService : IActorSystemService
    {
        public ActorSystemService()
        {
            // TODO: create a connection to the ActorSystem
        }

        public void SendMeasurement(Guid deviceId, MeterReadingReceived message)
        {
            throw new NotImplementedException();
        }
    }
}
