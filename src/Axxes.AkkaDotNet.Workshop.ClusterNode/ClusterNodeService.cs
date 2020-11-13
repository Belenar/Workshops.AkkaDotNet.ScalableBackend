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

        public ClusterNodeService(ILogger<ClusterNodeService> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // Read akka.hocon
            var hoconConfig = ConfigurationFactory.ParseString(await File.ReadAllTextAsync("akka.hocon", cancellationToken));

            // Get the ActorSystem Name
            var systemConfig = hoconConfig.GetConfig("system-settings");
            var actorSystemName = systemConfig.GetString("actorsystem-name");

            _actorSystem = ActorSystem.Create(actorSystemName, hoconConfig);

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var deviceId = Guid.NewGuid();

            var deviceManagerProps = DeviceManagerActor.CreateProps();
            var deviceManager = _actorSystem.ActorOf(deviceManagerProps, "devices");

            var deviceConnected = deviceManager.Ask<DeviceConnected>(new ConnectDevice(deviceId));
            var deviceActor = deviceConnected.Result.DeviceRef;

            while (!stoppingToken.IsCancellationRequested)
            {
                deviceActor.Tell(new MeterReadingReceived(deviceId, DateTime.UtcNow, 0.0M));

                //_logger.LogInformation("SeedNodeService running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
