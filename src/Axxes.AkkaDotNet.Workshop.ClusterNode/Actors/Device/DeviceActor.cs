using System;
using Akka.Actor;
using Akka.Event;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

internal class DeviceActor : ReceiveActor
{
    private const string NormalizationActorName = "value-normalization";
    private const string StorageActorName = "value-storage";

    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
        CreateChildren();
        Receive<MeterReadingReceived>(HandleMeterReading);
        Receive<NormalizedMeterReading>(HandleNormalizedReading);
        Receive<RequestLastNormalizedReadings>(Context.Child(StorageActorName).Forward);
    }

    private void CreateChildren()
    {
        var normalizationProps = ValueNormalizationActor.CreateProps();
        Context.ActorOf(normalizationProps, NormalizationActorName);
        var storageProps = ReadingStorageActor.CreateProps(_deviceId);
        Context.ActorOf(storageProps, StorageActorName);
    }

    private void HandleMeterReading(MeterReadingReceived reading)
    {
        Context.Child(NormalizationActorName).Tell(reading);
    }

    private void HandleNormalizedReading(NormalizedMeterReading reading)
    {
        Context.GetLogger().Debug($"Normalized reading received: {reading.MeterReading}");
        Context.Child(StorageActorName).Tell(reading);
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}

