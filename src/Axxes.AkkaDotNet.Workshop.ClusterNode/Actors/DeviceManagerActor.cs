using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

public class DeviceManagerActor : ReceiveActor
{
    private readonly List<Guid> _managedDevices = new();

    public DeviceManagerActor()
    {
        Receive<ConnectDevice>(ConnectNewDevice);
        Receive<GetAllDeviceIds>(HandleGetAllDeviceIds);
    }

    private void ConnectNewDevice(ConnectDevice connectDevice)
    {
        var deviceActor = Context.Child($"device-{connectDevice.DeviceId}");

        if (deviceActor.IsNobody())
        {
            var props = DeviceActor.CreateProps(connectDevice.DeviceId);
            deviceActor = Context.ActorOf(props, $"device-{connectDevice.DeviceId}");
            Context.GetLogger().Info($"Number of children created: {Context.GetChildren().Count()}");
            _managedDevices.Add(connectDevice.DeviceId);
        }

        Sender.Tell(new DeviceConnected(connectDevice.DeviceId, deviceActor));
    }

    private void HandleGetAllDeviceIds(GetAllDeviceIds obj)
    {
        Sender.Tell(new AllDeviceIds(_managedDevices.ToImmutableArray()));
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