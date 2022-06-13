using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Axxes.AkkaDotNet.Workshop.MessageReader.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.System
{
    public class ActorSystemService : IActorSystemService
    {
        private readonly ActorSystem _system;
        private readonly Dictionary<Guid, IActorRef> _proxyActors = new();

        public ActorSystemService()
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            var systemName = config.GetConfig("system-settings").GetString("system-name");

            _system = ActorSystem.Create(systemName, config);
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
            var props = DeviceProxyActor.CreateProps(deviceId);
            _proxyActors[deviceId] =
                _system.ActorOf(props, $"device-proxy-{deviceId}");
        }
    }
}
