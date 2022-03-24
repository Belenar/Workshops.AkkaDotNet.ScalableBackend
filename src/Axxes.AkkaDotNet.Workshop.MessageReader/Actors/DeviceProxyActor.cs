using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.Actors;

internal class DeviceProxyActor : ReceiveActor, IWithUnboundedStash
{
    private readonly Guid _deviceId;
    private IActorRef _deviceActor;

    public DeviceProxyActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Become(Started);
    }

    private void Started()
    {
        Receive<DeviceConnected>(HandleDeviceConnected);
        Receive<MeterReadingReceived>(StashMeterReadingReceived);
    }

    private void Connected()
    {
        Receive<MeterReadingReceived>(HandleMeterReadingReceived);
    }

    private void HandleDeviceConnected(DeviceConnected message)
    {
        _deviceActor = message.DeviceActor;
        Become(Connected);
        Stash.UnstashAll();
    }

    private void StashMeterReadingReceived(MeterReadingReceived obj)
    {
        Stash.Stash();
    }

    private void HandleMeterReadingReceived(MeterReadingReceived message)
    {
        _deviceActor.Tell(message);
    }

    protected override void PreStart()
    {
        var deviceManagerAddress = "/user/devices";
        var deviceManager = Context.ActorSelection(deviceManagerAddress);

        deviceManager.Tell(new ConnectDevice(_deviceId));

        base.PreStart();
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceProxyActor>(deviceId);
    }

    public IStash Stash { get; set; }
}