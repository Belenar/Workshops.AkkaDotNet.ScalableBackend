using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device
{
    class DeviceActor : ReceiveActor
    {
        private readonly Guid _deviceId;

        public DeviceActor(Guid deviceId)
        {
            _deviceId = deviceId;
            Receive<MeterReadingReceived>(HandleMeterReadingReceived);
        }

        private void HandleMeterReadingReceived(MeterReadingReceived obj)
        {
            Context.GetLogger().Info($"MeterReading handled in DeviceActor {_deviceId}.");
        }

        public static Props CreateProps(Guid deviceId)
        {
            return Props.Create<DeviceActor>(deviceId);
        }
    }
}
