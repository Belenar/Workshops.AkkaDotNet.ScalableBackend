using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Axxes.AkkaDotNet.Workshop.MessageReader.IotHub;
using Axxes.AkkaDotNet.Workshop.MessageReader.System;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Azure;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Axxes.AkkaDotNet.Workshop.MessageReader
{
    public class MessageReaderService : BackgroundService
    {
        private readonly ILogger<MessageReaderService> _logger;
        private readonly IotHubSettings _iotHubSettings;
        private readonly IActorSystemService _actorSystem;
        private EventProcessorClient _eventHubClient;

        public MessageReaderService(ILogger<MessageReaderService> logger, IotHubSettings iotHubSettings,
            IActorSystemService actorSystem)
        {
            _logger = logger;
            _iotHubSettings = iotHubSettings;
            _actorSystem = actorSystem;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Creating a client for the EventHub");
            try
            {
                // Create a random storage container, so the full stream will be read. DO NOT DO THIS for production. 
                var storageClient = await CreateBlobContainerClient();

                var consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

                _eventHubClient = new EventProcessorClient(
                    storageClient, 
                    consumerGroup, 
                    _iotHubSettings.EventHubConnectionString);

                _eventHubClient.ProcessEventAsync += ProcessEventAsync;
                _eventHubClient.ProcessErrorAsync += ProcessErrorAsync;

                await _eventHubClient.StartProcessingAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private async Task<BlobContainerClient> CreateBlobContainerClient()
        {
            var blobClient = new BlobServiceClient(_iotHubSettings.BlobStorageConnectionString);

            var containerName = $"checkpoint-{Guid.NewGuid()}";

            try
            {
                BlobContainerClient container = await blobClient.CreateBlobContainerAsync(containerName);

                if (await container.ExistsAsync())
                {
                    _logger.LogInformation("Created container {0}", container.Name);
                    return container;
                }
            }
            catch (RequestFailedException)
            {
                _logger.LogError("Failed to create BLOB container.");
                throw new Exception("Failed to create BLOB container.");
            }

            _logger.LogError("Failed to create BLOB container.");
            throw new Exception("Failed to create BLOB container.");
        }

        private async Task ProcessEventAsync(ProcessEventArgs arg)
        {
            Guid.TryParse(arg.Data.SystemProperties["iothub-connection-device-id"].ToString(), out var deviceId);

            var message = GetMessageContent(arg.Data);
            _actorSystem.SendMeasurement(deviceId, message);

            await arg.UpdateCheckpointAsync(arg.CancellationToken);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError($"Partition '{arg.PartitionId}': an unhandled exception was encountered: {arg.Exception.Message}");
            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _eventHubClient.StopProcessingAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        private static MeterReadingReceived GetMessageContent(EventData eventData)
        {
            var data = Encoding.UTF8.GetString(eventData.Body.ToArray());

            return JsonConvert.DeserializeObject<MeterReadingReceived>(data);
        }
    }
}
