using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Axxes.AkkaDotNet.Workshop.MessageReader.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.System
{
    public class ActorSystemService : IActorSystemService
    {
        private readonly ActorSystem _system;
        private Dictionary<Guid, IActorRef> _deviceProxies = new();

        public ActorSystemService()
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));
             
            var name = config.GetConfig("system-settings").GetString("actorsystem-name");
             
            _system = ActorSystem.Create(name, config);

            var routerProps = Props.Empty.WithRouter(FromConfig.Instance);
            _system.ActorOf(routerProps, "devices");
        }

        public void SendMeasurement(Guid deviceId, MeterReadingReceived message)
        {
            CreateProxyIfNotExists(deviceId);

            _deviceProxies[deviceId].Tell(message);
        }

        private void CreateProxyIfNotExists(Guid deviceId)
        {
            if(_deviceProxies.ContainsKey(deviceId))
                return;

            var props = DeviceProxyActor.CreateProps(deviceId);

            _deviceProxies[deviceId] = _system.ActorOf(props, $"proxy-{deviceId}");
        }
    }
}
