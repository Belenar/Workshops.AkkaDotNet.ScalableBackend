using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode
{
    public class ClusterNodeService : BackgroundService
    {
        private readonly ILogger<ClusterNodeService> _logger;

        public ClusterNodeService(ILogger<ClusterNodeService> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: create an ActorSystem here
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // TODO: Gracefully exit the ActorSystem here
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: Create base actors and send some messages to them
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("SeedNodeService running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
