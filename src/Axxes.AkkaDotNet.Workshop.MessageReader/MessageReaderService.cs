using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Axxes.AkkaDotNet.Workshop.MessageReader.IotHub;
using Axxes.AkkaDotNet.Workshop.MessageReader.System;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Microsoft.Azure.EventHubs;
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
        private EventHubClient _eventHubClient;

        public MessageReaderService(ILogger<MessageReaderService> logger, IotHubSettings iotHubSettings, IActorSystemService actorSystem)
        {
            _logger = logger;
            _iotHubSettings = iotHubSettings;
            _actorSystem = actorSystem;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Creating a client for the EventHub");

            var connectionString = new EventHubsConnectionStringBuilder(
                new Uri(_iotHubSettings.EventHubEndpoint),
                _iotHubSettings.EventHubPath,
                _iotHubSettings.SasKeyName,
                _iotHubSettings.SasKey);

            _eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());

            // Create a PartitionReceiver for each partition.
            var runtimeInfo = await _eventHubClient.GetRuntimeInformationAsync();
            var partitionIds = runtimeInfo.PartitionIds;

            var partitionReceivers = new List<Task>();
            foreach (string partition in partitionIds)
            {
                partitionReceivers.Add(ReceiveMessagesFromPartitionAsync(partition, stoppingToken));
            }

            // Wait for all the PartitionReceivers to finish.
            Task.WaitAll(partitionReceivers.ToArray());
        }

        private async Task ReceiveMessagesFromPartitionAsync(string partition, CancellationToken ct)
        {
            // Create the receiver using the default consumer group.
            var getMessagesSince = DateTime.UtcNow.AddDays(-1);
            var eventHubReceiver = _eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(getMessagesSince));

            _logger.LogInformation($"Creating a receiver on partition {partition}");
            
            while (true)
            {
                if (ct.IsCancellationRequested) break;

                _logger.LogDebug($"Listening for messages on {partition}");

                var events = await eventHubReceiver.ReceiveAsync(100);

                if (events == null) continue;

                foreach (var eventData in events)
                {
                    Guid.TryParse(eventData.SystemProperties["iothub-connection-device-id"].ToString(), out var deviceId);

                    var message = GetMessageContent(eventData);

                    _actorSystem.SendMeasurement(deviceId, message);
                }
            }
        }

        private static MeterReadingReceived GetMessageContent(EventData eventData)
        {
            var data = Encoding.UTF8.GetString(eventData.Body.Array ?? Array.Empty<byte>());

            return JsonConvert.DeserializeObject<MeterReadingReceived>(data);
        }
    }
}
