using System.Text.Json;
using Vertr.Exchange.Domain.Binary;
using Vertr.Exchange.Domain.Enums;
using Vertr.Exchange.Domain.Reports;

namespace Vertr.Exchange.Domain.Processors;
internal class BinaryCommandsProcessor
{
    private readonly Action<OrderCommand, BinaryCommand>? _binarycommandHandler;
    private readonly Action<OrderCommand, ReportQuery>? _reportQueryHandler;

    public BinaryCommandsProcessor(
        Action<OrderCommand, BinaryCommand> binarycommandHandler,
        Action<OrderCommand, ReportQuery> reportQueryHandler)
    {
        _binarycommandHandler = binarycommandHandler;
        _reportQueryHandler = reportQueryHandler;
    }

    public CommandResultCode AcceptBinaryCommand(OrderCommand cmd)
    {
        if (cmd.Command is OrderCommandType.BINARY_DATA_COMMAND)
        {
            var command = RestoreCommand(cmd.BinaryCommandType, cmd.BinaryData);

            if (command != null)
            {
                _binarycommandHandler?.Invoke(cmd, command);
            }
        }

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

    private BinaryCommand? RestoreCommand(BinaryDataType commandType, byte[] data)
    {
        return commandType switch
        {
            BinaryDataType.COMMAND_ADD_ACCOUNTS
                => JsonSerializer.Deserialize<BatchAddAccountsCommand>(data),
            BinaryDataType.COMMAND_ADD_SYMBOLS
                => JsonSerializer.Deserialize<BatchAddSymbolsCommand>(data),
            BinaryDataType.NONE => null,
            _ => null,// TODO: Handle unknown command type
        };
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
