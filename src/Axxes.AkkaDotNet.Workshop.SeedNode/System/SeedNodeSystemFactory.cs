using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Petabridge.Cmd;
using Petabridge.Cmd.Cluster;
using Petabridge.Cmd.Host;
using Petabridge.Cmd.Remote;

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

            var system = ActorSystem.Create(actorSystemName, clusterConfig);

            var pbm = PetabridgeCmd.Get(system);
            pbm.RegisterCommandPalette(new RemoteCommands());
            pbm.RegisterCommandPalette(ClusterCommands.Instance);
            pbm.Start();

            return system;
        }
    }
}