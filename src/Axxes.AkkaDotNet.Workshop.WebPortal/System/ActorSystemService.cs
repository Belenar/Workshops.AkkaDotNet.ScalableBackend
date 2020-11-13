using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Axxes.AkkaDotNet.Workshop.WebPortal.Actors;
using Axxes.AkkaDotNet.Workshop.WebPortal.Models;

namespace Axxes.AkkaDotNet.Workshop.WebPortal.System
{
    public class ActorSystemService : IActorSystemService
    {
        private ActorSystem _actorSystem;
        private IActorRef _devicesBroadcastRouter;
        private IActorRef _allDevicesActor;

        public ActorSystemService()
        {
            // Read akka.hocon
            var hoconConfig = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            // Get the ActorSystem Name
            var systemConfig = hoconConfig.GetConfig("system-settings");
            var actorSystemName = systemConfig.GetString("actorsystem-name");

            _actorSystem = ActorSystem.Create(actorSystemName, hoconConfig);

            _devicesBroadcastRouter = _actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "devices-broadcast");
            _allDevicesActor = _actorSystem.ActorOf(AllDevicesActor.CreateProps(_devicesBroadcastRouter), "all-devices");
        }

        public async Task<IEnumerable<Measurement>> GetMeasurements(Guid deviceId, DateTime fromDateTimeUtc, DateTime toDateTimeUtc)
        {
            var deviceActor = await _allDevicesActor.Ask<DeviceConnected>(new ConnectDevice(deviceId));

            // TODO: fetch the data from the ActorSystem
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            var allDeviceIds = await _allDevicesActor.Ask<AllDeviceIds>(new GetAllDeviceIds());

            var devices = new List<Device>();
            foreach (var deviceId in allDeviceIds.DeviceIds)
            {
                devices.Add(new Device {Id = deviceId, Name = $"Device {deviceId}"});
            }

            return devices;
        }
    }
}
