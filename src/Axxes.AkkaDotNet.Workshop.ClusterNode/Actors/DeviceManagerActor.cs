using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

public class DeviceManagerActor : ReceiveActor
{
    public DeviceManagerActor()
    {
        Receive<ConnectDevice>(HandleConnectDevice);
    }

    private void HandleConnectDevice(ConnectDevice message)
    {
        var childName = $"device-{message.DeviceId}";
        var child = Context.Child(childName);
        if (child.IsNobody())
        {
            var props = DeviceActor.CreateProps(message.DeviceId);
            child = Context.ActorOf(props, childName);
            Context.GetLogger().Log(LogLevel.InfoLevel, "Device Actor Created:{0}", childName);
        }
        Sender.Tell(new DeviceConnected(child));
    }

    protected override SupervisorStrategy SupervisorStrategy()
    {
        return new OneForOneStrategy(ex =>
        {
            if (ex is DivideByZeroException)
                return Directive.Stop;
            else
                return Directive.Resume;
        });
    }
}
