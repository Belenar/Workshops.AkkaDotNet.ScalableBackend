using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode
{
    public class ClusterNodeService : BackgroundService
    {
        private readonly ILogger<ClusterNodeService> _logger;
        private ActorSystem _system;
        private IActorRef _deviceManager;

        public ClusterNodeService(ILogger<ClusterNodeService> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            var name = config.GetConfig("system-settings").GetString("actorsystem-name");

            _system = ActorSystem.Create(name, config);

            var deviceManagerProps = DeviceManagerActor.CreateProps();
            _deviceManager = _system.ActorOf(deviceManagerProps, "devices");

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            var shutdown = CoordinatedShutdown.Get(_system);
            await shutdown.Run(CoordinatedShutdown.ClrExitReason.Instance);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: Create base actors and send some messages to them
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
