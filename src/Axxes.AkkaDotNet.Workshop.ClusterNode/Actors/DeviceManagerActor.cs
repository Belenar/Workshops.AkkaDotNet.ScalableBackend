using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

public class DeviceManagerActor : ReceiveActor
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

    protected override SupervisorStrategy SupervisorStrategy()
    {
        var strategy = new OneForOneStrategy(10, 5000, exception =>
        {
            if (exception is NotImplementedException)
                return Directive.Resume;
            return Directive.Restart;
        });
        return strategy;
    }

    public static Props CreateProps()
    {
        return Props.Create<DeviceManagerActor>();
    }
}