using System;
using System.Linq;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Dapper;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

public class ColdStorageActor: ReceiveActor
{
    private readonly Guid _deviceId;

    public ColdStorageActor(Guid deviceId)
    {
        _deviceId = deviceId;
        Receive<WriteReadingsToDatabase>(WriteReadingsToDatabase);
    }

    private void WriteReadingsToDatabase(WriteReadingsToDatabase readings)
    {
        using var connection = DatabaseConnectionFactory.GetHistoryConnection();

        var query = @"INSERT INTO [dbo].[MeterReadings] ([DeviceId], [Timestamp], [MeterReading], [Consumption])
                    VALUES (@DeviceId, @Timestamp, @MeterReading, @Consumption);";

        var data = readings.Readings.Select(reading =>
            new { DeviceId = _deviceId, reading.Timestamp, reading.MeterReading, reading.Consumption });

        connection.Execute(query, data);

        var lastDate = readings.Readings.Max(r => r.Timestamp);

        Sender.Tell(new WrittenReadingsToDatabase(lastDate));
    }

    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<ColdStorageActor>(deviceId);
    }
}