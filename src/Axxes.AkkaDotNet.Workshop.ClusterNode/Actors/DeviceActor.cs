using System;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors;

internal class DeviceActor : ReceiveActor
{
    private const string ValueNormalizationName = "value-normaization";
    private const string ValueStorageName = "value-storage";
    private readonly Guid _deviceId;

    public DeviceActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Receive<MeterReadingReceived>(HandleMeterReadingReceived);
        Receive<NormalizedMeterReading>(HandleNormalizedMeterReadingReceived);
        Receive<RequestLastNormalizedReadings>(HandleRequestLastNormalizedReadings);
        Receive<QueryMeasurementsData>(msg => Context.Child(ValueStorageName).Forward(msg));
        CreateChildren();
    }

    private void CreateChildren()
    {
        // value-normalization
        var normalizationProps = ValueNormalizationActor.CreateProps();
        Context.ActorOf(normalizationProps, ValueNormalizationName);

        // value-storage
        var storageProps = ValueStorageActor.CreateProps(_deviceId);
        Context.ActorOf(storageProps, ValueStorageName);
    }

    private void HandleMeterReadingReceived(MeterReadingReceived message)
    {
        Context.Child(ValueNormalizationName).Forward(message);
    }

    private void HandleNormalizedMeterReadingReceived(NormalizedMeterReading message)
    {
        Context.Child(ValueStorageName).Forward(message);
    }

    private void HandleRequestLastNormalizedReadings(RequestLastNormalizedReadings message)
    {
        Context.Child(ValueStorageName).Forward(message);
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<DeviceActor>(deviceId);
    }
}