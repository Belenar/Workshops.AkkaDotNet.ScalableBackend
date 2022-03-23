using System;

namespace Axxes.AkkaDotNet.Workshop.Shared.Messages;

    public class ConnectDevice
    {
        public Guid DeviceId { get; }

        public ConnectDevice(Guid deviceId)
        {
            DeviceId = deviceId;
        }
    }