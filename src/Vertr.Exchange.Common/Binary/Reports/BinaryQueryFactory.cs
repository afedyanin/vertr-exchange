using System.Text.Json;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.Common.Binary.Reports;

public static class BinaryQueryFactory
{
    public static IBinaryQuery? GetBinaryQuery(BinaryDataType commandType, byte[] data)
    {
#pragma warning disable IDE0072 // Add missing cases
        return commandType switch
        {
            BinaryDataType.NONE => null,
            BinaryDataType.QUERY_STATE_HASH => throw new NotImplementedException(),
            BinaryDataType.QUERY_SINGLE_USER_REPORT
                => JsonSerializer.Deserialize<SingleUserReportQuery>(data),
            BinaryDataType.QUERY_TOTAL_CURRENCY_BALANCE => throw new NotImplementedException(),
            _ => null,// TODO: Handle unknown command type
        };
#pragma warning restore IDE0072 // Add missing cases
    }

    public static SingleUserReportResult? GetSingleUserReportResult(OrderCommand command)
    {
        if (command == null || command.BinaryCommandType != BinaryDataType.QUERY_SINGLE_USER_REPORT)
        {
            return null;
        }

        var binaryEvent = EventsHelper.GetFirstBinaryEvent(command);

        if (binaryEvent == null)
        {
            return null;
        }

        var report = JsonSerializer.Deserialize<SingleUserReportResult>(binaryEvent.BinaryData);

        return report;
    }
}
