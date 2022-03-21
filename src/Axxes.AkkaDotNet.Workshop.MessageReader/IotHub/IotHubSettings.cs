using Microsoft.Extensions.Configuration;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.IotHub
{
    public class IotHubSettings
    {
        public IotHubSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("IoTHub");
            EventHubConnectionString = section.GetValue<string>("EventHubConnectionString");
            BlobStorageConnectionString = section.GetValue<string>("BlobStorageConnectionString");
        }
        public string EventHubConnectionString { get; }
        public string BlobStorageConnectionString { get; }
    }
}
