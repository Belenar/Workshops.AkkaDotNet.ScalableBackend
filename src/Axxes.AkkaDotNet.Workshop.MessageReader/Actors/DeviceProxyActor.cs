using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.Actors;

public class DeviceProxyActor : ReceiveActor, IWithUnboundedStash
{
    private readonly Guid _deviceId;
    private readonly IActorRef _deviceManager;
    private IActorRef _deviceActor;

    public DeviceProxyActor(Guid deviceId, IActorRef deviceManager)
    {
        _deviceId = deviceId;
        _deviceManager = deviceManager;
        Become(Started);
    }

    protected override void PreStart()
    {
        _deviceManager.Tell(new ConnectDevice(_deviceId));
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

    public static Props CreateProps(Guid deviceId, IActorRef deviceManager)
    {
        return Props.Create<DeviceProxyActor>(deviceId, deviceManager);
    }

    public IStash Stash { get; set; }
}