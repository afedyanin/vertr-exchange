using System.Text;
using System.Text.Json;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common.Binary.Reports;
using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common.Reports;

namespace Vertr.Exchange.Application.Commands.Queries;

public class SingleUserReport(
    long orderId,
    DateTime timestamp,
    long uid) : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.BINARY_DATA_QUERY;

    private readonly long _uid = uid;

    public override void Fill(ref OrderCommand command)
    {
        var cmd = new SingleUserReportQuery()
        {
            Uid = _uid,
        };

        base.Fill(ref command);
        command.Uid = _uid;
        command.BinaryCommandType = BinaryDataType.QUERY_SINGLE_USER_REPORT;
        command.BinaryData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cmd));
    }

    public SingleUserReportResult? GetResult(IApiCommandResult commandResult)
    {
        if (commandResult.OrderId != OrderId)
        {
            // throw Exception?
            return null;
        }

        if (commandResult.ResultCode != CommandResultCode.SUCCESS)
        {
            return null;
        }

        var binaryEvent = commandResult.RootEvent;

        if (binaryEvent == null)
        {
            return null;
        }

        if (binaryEvent.EventType != EngineEventType.BINARY_EVENT)
        {
            return null;
        }

        var report = JsonSerializer.Deserialize<SingleUserReportResult>(binaryEvent.BinaryData);
        return report;
    }
}
