using Microsoft.Extensions.Configuration;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.IotHub
{
    public class IotHubSettings
    {
        public IotHubSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("IoTHub");
            EventHubEndpoint = section.GetValue<string>("EventHubEndpoint");
            EventHubPath = section.GetValue<string>("EventHubPath");
            SasKeyName = section.GetValue<string>("SasKeyName");
            SasKey = section.GetValue<string>("SasKey");
        }
        public string EventHubEndpoint { get; }
        public string EventHubPath { get; }
        public string SasKeyName { get; }
        public string SasKey { get; }
    }
}
