using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.Actors;

public class DeviceProxyActor : ReceiveActor, IWithUnboundedStash
{
    private readonly Guid _deviceId;
    private IActorRef _deviceActor;

    public DeviceProxyActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Become(Started);
    }

    protected override void PreStart()
    {
        var deviceManager =
            Context.ActorSelection("akka.tcp://WorkshopSystem@localhost:8081/user/device-manager");
        deviceManager.Tell(new ConnectDevice(_deviceId));
    }

    private void Started()
    {
        Receive<DeviceConnected>(HandleDeviceConnected);
        Receive<MeterReadingReceived>(StashMeterReading);
    }

    private void StashMeterReading(MeterReadingReceived message)
    {
        Stash.Stash();
    }

    private void Initialized()
    {
        Receive<MeterReadingReceived>(ForwardMeterReading);
    }

    private void HandleDeviceConnected(DeviceConnected message)
    {
        _deviceActor = message.DeviceActor;
        Become(Initialized);
        Stash.UnstashAll();
    }

    private void ForwardMeterReading(MeterReadingReceived message)
    {
        _deviceActor.Tell(message);
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceProxyActor>(deviceId);
    }

    public IStash Stash { get; set; }
}