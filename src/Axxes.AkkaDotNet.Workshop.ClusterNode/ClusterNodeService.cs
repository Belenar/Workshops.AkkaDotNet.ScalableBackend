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
        private ActorSystem _system;

        public ClusterNodeService(ILogger<ClusterNodeService> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var config = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            var systemName = config.GetConfig("system-settings").GetString("system-name");

            _system = ActorSystem.Create(systemName, config);
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
            var deviceManagerProps = Props.Create<DeviceManagerActor>();
            var deviceManager = _system.ActorOf(deviceManagerProps, "device-manager");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("SeedNodeService running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
