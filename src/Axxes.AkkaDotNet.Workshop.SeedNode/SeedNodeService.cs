using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.SeedNode.System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Petabridge.Cmd.Cluster;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;

namespace Axxes.AkkaDotNet.Workshop.SeedNode
{
    public class SeedNodeService : BackgroundService
    {
        private readonly ILogger<SeedNodeService> _logger;
        private ActorSystem _system;

        public SeedNodeService(ILogger<SeedNodeService> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            _system = SeedNodeSystemFactory.CreateActorSystem();

            var pbm = PetabridgeCmd.Get(_system);
            // enable Akka.Cluster management commands
            pbm.RegisterCommandPalette(ClusterCommands.Instance);
            // enable Akka.Remote management commands
            pbm.RegisterCommandPalette(RemoteCommands.Instance);
            pbm.Start();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("SeedNodeService running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
