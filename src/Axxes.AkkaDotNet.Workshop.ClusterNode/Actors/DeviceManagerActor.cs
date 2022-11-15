using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class DeviceManagerActor : ReceiveActor
{
    public DeviceManagerActor()
    {
        Receive<ConnectDevice>(HandleConnectDevice);
    }

    private void HandleConnectDevice(ConnectDevice connectDevice)
    {
        var deviceId = connectDevice.DeviceId;
        var actorName = $"device-{deviceId}";

        var childActor = Context.Child(actorName);

        if (childActor == ActorRefs.Nobody)
        {
            var deviceProps = DeviceActor.CreateProps(deviceId);
            childActor = Context.ActorOf(deviceProps, actorName);
        }
        
        Sender.Tell(new DeviceConnected(deviceId, childActor));
    }
}