using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.MessageReader.Actors;

internal class DeviceProxyActor : ReceiveActor, IWithUnboundedStash
{
    public record RetryConnect;

    private readonly Guid _deviceId;
    private readonly IActorRef _deviceManagerRouter;
    private IActorRef _deviceActor;
    private Cancelable _cancelRetries;

    public DeviceProxyActor(Guid deviceId, IActorRef deviceManagerRouter)
    {
        _deviceId = deviceId;
        _deviceManagerRouter = deviceManagerRouter;
        Become(Started);
    }

    private void Started()
    {
        Receive<DeviceConnected>(HandleDeviceConnected);
        Receive<MeterReadingReceived>(StashMeterReading);
        Receive<RetryConnect>(_ => ConnectDevice());
    }

    private void Initialized()
    {
        Receive<MeterReadingReceived>(ForwardMeterReading);
    }

    private void HandleDeviceConnected(DeviceConnected deviceConnected)
    {
        _deviceActor = deviceConnected.DeviceActor;
        _cancelRetries.Cancel();
        Become(Initialized);
        Stash.UnstashAll();
    }

    protected override void PreStart()
    {
        ConnectDevice();

        // Schedule a check message that will retry the connect every 30 seconds
        _cancelRetries = new Cancelable(Context.System.Scheduler);

        var delay = new TimeSpan(0, 0, 30);
        Context.System.Scheduler.ScheduleTellRepeatedly(
            initialDelay: delay,
            interval: delay,
            sender: Self,
            receiver: Self,
            message: new RetryConnect(),
            cancelable: _cancelRetries);
    }

    private void ConnectDevice()
    {
        if (_deviceActor != null)
            return;

        _deviceManagerRouter.Tell(new ConnectDevice(_deviceId));
    }

    private void ForwardMeterReading(MeterReadingReceived meterReading)
    {
        _deviceActor.Tell(meterReading);
    }

    private void StashMeterReading(MeterReadingReceived meterReading)
    {
        Stash.Stash();
    }

    public static Props CreateProps(Guid deviceId, IActorRef deviceManagerRouter)
    {
        return Props.Create<DeviceProxyActor>(deviceId, deviceManagerRouter);
    }

    public IStash Stash { get; set; }
}

