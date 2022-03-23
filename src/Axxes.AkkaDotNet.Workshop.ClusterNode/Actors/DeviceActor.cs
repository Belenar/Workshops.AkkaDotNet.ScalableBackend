using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class DeviceActor : ReceiveActor
{
    private const string ValueNormalizationName = "value-normaization";
    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Receive<MeterReadingReceived>(HandleMeterReadingReceived);
        Receive<NormalizedMeterReading>(HandleNormalizedMeterReadingReceived);
        CreateChildren();
    }

    private void CreateChildren()
    {
        // value-normalization
        var props = ValueNormalizationActor.CreateProps();
        Context.ActorOf(props, ValueNormalizationName);
    }

    private void HandleMeterReadingReceived(MeterReadingReceived message)
    {
        Context.Child(ValueNormalizationName).Tell(message);
    }

    private void HandleNormalizedMeterReadingReceived(NormalizedMeterReading message)
    {
        Context.GetLogger().Info($"Device {_deviceId} - Time: {message.Timestamp} - Reading: {message.MeterReading}");
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}