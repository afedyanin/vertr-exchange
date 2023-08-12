using System.Text.Json;
using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain.Reports;

internal sealed class ReportQueryProcessor
{
    private readonly Action<OrderCommand, ReportQuery>? _reportQueryHandler;

    public ReportQueryProcessor(Action<OrderCommand, ReportQuery>? reportQueryHandler)
    {
        _reportQueryHandler = reportQueryHandler;
    }

    public CommandResultCode AcceptReportQuery(OrderCommand cmd)
    {
        if (cmd.Command is OrderCommandType.BINARY_DATA_QUERY)
        {
            var query = RestoreQuery(cmd.BinaryCommandType, cmd.BinaryData);

            if (query != null)
            {
                _reportQueryHandler?.Invoke(cmd, query);
            }
        }

        return CommandResultCode.SUCCESS;
    }

    private ReportQuery? RestoreQuery(BinaryDataType commandType, byte[] data)
    {
        return commandType switch
        {
            BinaryDataType.QUERY_SINGLE_USER_REPORT
                => JsonSerializer.Deserialize<SingleUserReportQuery>(data),
            BinaryDataType.QUERY_TOTAL_CURRENCY_BALANCE
                => JsonSerializer.Deserialize<TotalCurrencyBalanceReportQuery>(data),
            BinaryDataType.QUERY_STATE_HASH
                => JsonSerializer.Deserialize<StateHashReportQuery>(data),
            BinaryDataType.NONE => null,
            _ => null,// TODO: Handle unknown command type
        };
    }
}
