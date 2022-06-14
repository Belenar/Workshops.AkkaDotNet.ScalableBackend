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
        private readonly Dictionary<Guid, IActorRef> _proxyActors = new();
        private readonly IActorRef _router;

        public ActorSystemService()
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            var systemName = config.GetConfig("system-settings").GetString("system-name");

            _system = ActorSystem.Create(systemName, config);

            // Create The device manager routing group:
            var props = Props.Empty.WithRouter(FromConfig.Instance);
            _router = _system.ActorOf(props, "device-router");
        }

        public void SendMeasurement(Guid deviceId, MeterReadingReceived message)
        {
            CreateProxyIfNotExists(deviceId);
            _proxyActors[deviceId].Tell(message);
        }

        private void CreateProxyIfNotExists(Guid deviceId)
        {
            if (_proxyActors.ContainsKey(deviceId))
                return;
            var props = DeviceProxyActor.CreateProps(deviceId, _router);
            _proxyActors[deviceId] =
                _system.ActorOf(props, $"device-proxy-{deviceId}");
        }
    }
}
