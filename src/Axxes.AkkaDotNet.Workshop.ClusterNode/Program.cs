using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                DatabaseConnectionFactory.HistoryConnectionString =
                    hostContext.Configuration.GetSection("ConnectionStrings").GetValue<string>("HistoryConnectionString");
                services.AddHostedService<ClusterNodeService>();
            });
}