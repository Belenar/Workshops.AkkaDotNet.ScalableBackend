﻿using System;
using System.Collections.Generic;
using System.IO;
using Akka.Actor;
using Akka.Configuration;
using System.Threading;
using Axxes.AkkaDotNet.Workshop.MessageReader.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Akka.Routing;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.System
{
    public class ActorSystemService : IActorSystemService
    {
        private readonly ActorSystem _actorSystem;
        private readonly Dictionary<Guid, IActorRef> _proxies = new();
        private readonly IActorRef _deviceManagerRouter;

        public ActorSystemService()
        {
            var configText = File.ReadAllText("akka.hocon");
            var config = ConfigurationFactory.ParseString(configText);

            var systemName = config.GetString("system-settings.actorsystem-name");

            _actorSystem = ActorSystem.Create(systemName, config);

            _deviceManagerRouter = _actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "device-managers");
        }

        public void SendMeasurement(Guid deviceId, MeterReadingReceived message)
        {
            if (!_proxies.ContainsKey(deviceId))
            {
                var props = DeviceProxyActor.CreateProps(deviceId, _deviceManagerRouter);
                var actorName = $"device-{deviceId}-proxy";
                _proxies[deviceId] = _actorSystem.ActorOf(props, actorName);
            }
            _proxies[deviceId].Tell(message);
        }
    }
}
