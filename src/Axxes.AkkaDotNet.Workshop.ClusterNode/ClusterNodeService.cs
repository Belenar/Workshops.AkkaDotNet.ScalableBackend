using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode
{
    public class ClusterNodeService : BackgroundService
    {
        private readonly ILogger<ClusterNodeService> _logger;
        private ActorSystem _actorSystem;
        private IActorRef _deviceManager;

        public ClusterNodeService(ILogger<ClusterNodeService> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var configText = File.ReadAllText("akka.hocon");
            var config = ConfigurationFactory.ParseString(configText);

            var systemName = config.GetString("system-settings.actorsystem-name");

            _actorSystem = ActorSystem.Create(systemName, config);

            var deviceManagerProps = Props.Create<DeviceManagerActor>();
            _deviceManager = _actorSystem.ActorOf(deviceManagerProps, "device-manager");

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            var shutdown = CoordinatedShutdown.Get(_actorSystem);
            await shutdown.Run(CoordinatedShutdown.ClrExitReason.Instance);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: Create base actors and send some messages to them
            while (!stoppingToken.IsCancellationRequested)
            {
                var connected = await _deviceManager.Ask<DeviceConnected>(new ConnectDevice(Guid.NewGuid()));
                //_logger.LogInformation("SeedNodeService running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
