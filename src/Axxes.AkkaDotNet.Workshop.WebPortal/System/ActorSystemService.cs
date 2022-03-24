using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly ActorSystem _system;
        private readonly IActorRef _allDevices;

        public ActorSystemService()
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            var name = config.GetConfig("system-settings").GetString("actorsystem-name");

            _system = ActorSystem.Create(name, config);

            var routerProps = Props.Empty.WithRouter(FromConfig.Instance);
            var broadcastRouter = _system.ActorOf(routerProps, "device-broadcast");

            _allDevices = _system.ActorOf(AllDevicesActor.CreateProps(broadcastRouter), "all-devices");
        }

        public IEnumerable<Measurement> GetMeasurements(Guid deviceId, in DateTime fromDateTimeUtc, in DateTime toDateTimeUtc)
        {
            // TODO: fetch the data from the ActorSystem
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            var allDevices = await _allDevices.Ask<AllDeviceIds>(new GetAllDeviceIds());
            return allDevices.DeviceIds.Select(id => new Device { Id = id, Name = $"device {id}" });
        }
    }
}
