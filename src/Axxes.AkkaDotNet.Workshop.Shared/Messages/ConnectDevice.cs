using System;
using Akka.Routing;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages
{
    public class ConnectDevice: IConsistentHashable
    {
        public Guid Id { get; }


        public ConnectDevice(Guid id)
        {
            Id = id;
        }

        public object ConsistentHashKey => Id;
    }
}