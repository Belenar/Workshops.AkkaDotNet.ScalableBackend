using System.IO;
using Akka.Actor;
using Akka.Configuration;

namespace Axxes.AkkaDotNet.Workshop.SeedNode.System
{
    static class SeedNodeSystemFactory
    {
        public static ActorSystem CreateActorSystem()
        {
            // Read akka.hocon
            var clusterConfig = ConfigurationFactory.ParseString(File.ReadAllText("akka.hocon"));

            // Get the ActorSystem Name
            var seedNodeConfig = clusterConfig.GetConfig("system-settings");
            var actorSystemName = seedNodeConfig.GetString("actorsystem-name");

            return ActorSystem.Create(actorSystemName, clusterConfig);
        }
    }
}