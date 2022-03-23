using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class DeviceManagerActor : ReceiveActor
{
    public DeviceManagerActor()
    {
        Receive<ConnectDevice>(ConnectNewDevice);
    }

    private void ConnectNewDevice(ConnectDevice connectDevice)
    {
        var deviceActor = Context.Child($"device-{connectDevice.DeviceId}");

        if (deviceActor.IsNobody())
        {
            var props = DeviceActor.CreateProps(connectDevice.DeviceId);
            deviceActor = Context.ActorOf(props, $"device-{connectDevice.DeviceId}");
        }

        Sender.Tell(new DeviceConnected(connectDevice.DeviceId, deviceActor));
    }

    public static Props CreateProps()
    {
        return Props.Create<DeviceManagerActor>();
    }
}

internal class DeviceActor : UntypedActor
{
    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
    }

    protected override void OnReceive(object message)
    {
        throw new NotImplementedException();
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}