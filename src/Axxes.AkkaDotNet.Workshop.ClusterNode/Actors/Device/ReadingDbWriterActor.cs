using System;
using System.Collections.Immutable;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Axxes.AkkaDotNet.Workshop.ClusterNode.Helpers;
using Axxes.AkkaDotNet.Workshop.Shared.Messages;
using Dapper;

namespace Axxes.AkkaDotNet.Workshop.ClusterNode.Actors.Device;

class ReadingDbWriterActor : ReceiveActor
{
    private readonly Guid _deviceId;

    public ReadingDbWriterActor(Guid deviceId)
    {
        _deviceId = deviceId;
        ReceiveAsync<WriteReadingsToDatabase>(HandleWriteReadingsToDatabase);
    }

    private async Task HandleWriteReadingsToDatabase(WriteReadingsToDatabase message)
    {
        await using var connection = DatabaseConnectionFactory.GetHistoryConnection();
        
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();
       
        await WriteReadings(message.Readings, connection, transaction);
                
        await transaction.CommitAsync();

        ReplyToSender(message);
    }

    private async Task WriteReadings(ImmutableArray<NormalizedMeterReading> messageReadings, SqlConnection connection, DbTransaction transaction)
    {
        var query = $@"INSERT INTO dbo.MeterReadings (DeviceId, Timestamp, MeterReading, Consumption)  
                            (SELECT '{_deviceId}', @Timestamp, @MeterReading, @Consumption
                            WHERE NOT EXISTS(SELECT DeviceId FROM dbo.MeterReadings r WHERE r.DeviceId = '{_deviceId}' AND r.Timestamp = @Timestamp)); ";
        await connection.ExecuteAsync(query, messageReadings, transaction);
    }

    private void ReplyToSender(WriteReadingsToDatabase message)
    {
        if (!message.Readings.Any())
            return;

        var lastValue = message.Readings.Max(r => r.Timestamp);
        Sender.Tell(new WrittenReadingsToDatabase(lastValue));
    }


    public static Props CreateProps(Guid deviceId)
    {
        return Props.Create<ReadingDbWriterActor>(deviceId);
    }
}
