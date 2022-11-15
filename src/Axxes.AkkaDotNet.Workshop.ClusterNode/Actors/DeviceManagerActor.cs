using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

public class DeviceManagerActor : ReceiveActor
{
    // private int _numberOfDeviceActors = 0;

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
            // _numberOfDeviceActors++;
            // Context.GetLogger().Debug($"Device actors created: {_numberOfDeviceActors}");
        }
        
        Sender.Tell(new DeviceConnected(deviceId, childActor));
    }
}