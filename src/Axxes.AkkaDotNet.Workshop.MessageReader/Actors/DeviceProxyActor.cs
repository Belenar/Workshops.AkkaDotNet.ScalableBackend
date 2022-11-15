using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.Actors;

internal class DeviceProxyActor : ReceiveActor
{
    private readonly Guid _deviceId;
    private IActorRef _deviceActor;

    public DeviceProxyActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Receive<MeterReadingReceived>(ForwardMeterReading);
        Receive<DeviceConnected>(HandleDeviceConnected);
    }

    private void HandleDeviceConnected(DeviceConnected deviceConnected)
    {
        _deviceActor = deviceConnected.DeviceActor;
    }

    protected override void PreStart()
    {
        // Don't use hardcoded strings in PROD!
        var selection = Context.ActorSelection("akka.tcp://WorkshopActorSystem@localhost:8081/user/device-manager");
        selection.Tell(new ConnectDevice(_deviceId));
    }

    private void ForwardMeterReading(MeterReadingReceived meterReading)
    {
        _deviceActor?.Tell(meterReading);
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceProxyActor>(deviceId);
    }
}

