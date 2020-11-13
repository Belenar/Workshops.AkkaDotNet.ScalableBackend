using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using Axxes.AkkaDotNet.Workshop.MessageReader.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.System
{
    public class ActorSystemService : IActorSystemService
    {
        private ActorSystem _actorSystem;
        private Dictionary<Guid, IActorRef> _proxyActors = new Dictionary<Guid, IActorRef>();
        private IActorRef _devicesRouter;

        public ActorSystemService()
        {
            // Read akka.hocon
            var hoconConfig = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            // Get the ActorSystem Name
            var systemConfig = hoconConfig.GetConfig("system-settings");
            
            var actorSystemName = systemConfig.GetString("actorsystem-name");
            SystemConstants.RemoteSystemAddress = systemConfig.GetString("remote-actorystem");

            _actorSystem = ActorSystem.Create(actorSystemName, hoconConfig);

            _devicesRouter = _actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "devices");
        }

        public void SendMeasurement(Guid deviceId, MeterReadingReceived message)
        {
            if (!_proxyActors.ContainsKey(deviceId))
                CreateProxyActor(deviceId);

            _proxyActors[deviceId].Tell(message);
        }

        private void CreateProxyActor(Guid deviceId)
        {
            var props = DeviceActorProxy.CreateProps(deviceId, _devicesRouter);
            _proxyActors[deviceId] = _actorSystem.ActorOf(props, $"proxy-{deviceId}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
