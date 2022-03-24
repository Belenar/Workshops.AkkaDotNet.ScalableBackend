using System;
using Akka.Routing;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages;

    public class ConnectDevice : IConsistentHashable
    {
        public Guid DeviceId { get; }

        public ConnectDevice(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public object ConsistentHashKey => DeviceId;
    }