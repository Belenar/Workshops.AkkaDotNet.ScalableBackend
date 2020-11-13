using System;
using System.Collections.Generic;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.System
{
    public class ActorSystemService : IActorSystemService
    {
        public ActorSystemService()
        {
            // TODO: create a connection to the ActorSystem
        }

        public IEnumerable<Measurement> GetMeasurements(Guid deviceId, in DateTime fromDateTimeUtc, in DateTime toDateTimeUtc)
        {
            // TODO: fetch the data from the ActorSystem
            throw new NotImplementedException();
        }

        public IEnumerable<Device> GetAllDevices()
        {
            // TODO: fetch the data from the ActorSystem
            throw new NotImplementedException();
        }
    }
}
