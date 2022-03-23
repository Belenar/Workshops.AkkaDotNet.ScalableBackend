using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Persistence;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors
{
    internal class ValueStorageActor : ReceivePersistentActor
    {
        public override string PersistenceId { get; }

        public ValueStorageActor(Guid deviceId)
        {
            PersistenceId = $"value-storage-{deviceId}";
            Command<NormalizedMeterReading>(HandleNormalizedMeterReading);
        }

        private void HandleNormalizedMeterReading(NormalizedMeterReading message)
        {
            Persist(message, msg => { });
        }

        public static Props CreateProps(Guid deviceId)
        {
            return Props.Create<ValueStorageActor>(deviceId);
        }
    }
}
