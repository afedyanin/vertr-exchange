using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Common.Binary.Reports;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.MatchingEngine.Binary;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;

[assembly: InternalsVisibleTo("Vertr.Exchange.MatchingEngine.Tests")]

namespace Vertr.Exchange.MatchingEngine;

public class OrderMatchingEngine(
    IOrderBookProvider orderBookProvider,
    IOptions<MatchingEngineConfiguration> options,
    ILogger<OrderMatchingEngine> logger) : IOrderMatchingEngine
{
    private readonly MatchingEngineConfiguration _config = options.Value;

    private readonly IOrderBookProvider _orderBookProvider = orderBookProvider;
    private readonly ILogger<OrderMatchingEngine> _logger = logger;

    public void ProcessOrder(long seq, OrderCommand cmd)
    {
        if (HasErrorResult(cmd))
        {
            _logger.LogInformation("Skip command={CommandType} OrderId={OrderId}. InvalidState={ResultCode}",
                cmd.Command,
                cmd.OrderId,
                cmd.ResultCode);

            return;
        }

        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
            case OrderCommandType.REDUCE_ORDER:
            case OrderCommandType.CANCEL_ORDER:
            case OrderCommandType.MOVE_ORDER:
            case OrderCommandType.ORDER_BOOK_REQUEST:
                _logger.LogDebug("Processing command={CommandType} OrderId={OrderId} Uid={Uid}", cmd.Command, cmd.OrderId, cmd.Uid);
                ProcessMatchingCommand(cmd, seq);
                break;
            case OrderCommandType.RESET:
                _logger.LogWarning("Processing RESET command OrderId={OrderId}", cmd.OrderId);
                _orderBookProvider.Reset();
                cmd.ResultCode = CommandResultCode.SUCCESS;
                break;
            case OrderCommandType.NOP:
                _logger.LogDebug("Processing NOP command. OrderId={OrderId}", cmd.OrderId);
                cmd.ResultCode = CommandResultCode.SUCCESS;
                break;
            case OrderCommandType.BINARY_DATA_COMMAND:
                _logger.LogDebug("Processing BinaryCommand={CommandType} OrderId={OrderId}", cmd.BinaryCommandType, cmd.OrderId);
                cmd.ResultCode = AcceptBinaryCommand(cmd);
                break;
            case OrderCommandType.BINARY_DATA_QUERY:
                _logger.LogDebug("Processing BinaryQuery={CommandType} OrderId={OrderId}", cmd.BinaryCommandType, cmd.OrderId);
                cmd.ResultCode = AcceptBinaryQuery(cmd);
                break;
            case OrderCommandType.ADD_USER:
            case OrderCommandType.SUSPEND_USER:
            case OrderCommandType.RESUME_USER:
            case OrderCommandType.BALANCE_ADJUSTMENT:
            case OrderCommandType.PERSIST_STATE_MATCHING:
            case OrderCommandType.PERSIST_STATE_RISK:
            case OrderCommandType.GROUPING_CONTROL:
            case OrderCommandType.SHUTDOWN_SIGNAL:
            case OrderCommandType.RESERVED_COMPRESSED:
                _logger.LogDebug("Skipping command={CommandType} OrderId={OrderId}", cmd.Command, cmd.OrderId);
                break;
            default:
                // TODO: How to handle exception here
                throw new NotSupportedException(cmd.Command.ToString());
        }
    }

    internal void ProcessMatchingCommand(OrderCommand cmd, long seq)
    {
        var orderBook = _orderBookProvider.GetOrderBook(cmd.Symbol);

        if (orderBook == null)
        {
            _logger.LogWarning("Order book not found for Symbol={Symbol} OrderId={OrderId}.",
                cmd.Symbol,
                cmd.OrderId);

            cmd.ResultCode = CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID;
            return;
        }

        var orderBookCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd, seq);

        // TODO: Catch and handle exceptions
        _logger.LogDebug("Executing orderCommand={commandType} OrderId={OrderId}",
            orderBookCommand.GetType(),
            cmd.OrderId);

        cmd.ResultCode = orderBookCommand.Execute();

        if (cmd.Command != OrderCommandType.ORDER_BOOK_REQUEST
            && cmd.ResultCode == CommandResultCode.SUCCESS)
        {
            _logger.LogDebug("Attach market data for OrderId={OrderId}", cmd.OrderId);
            cmd.MarketData = orderBook.GetL2MarketDataSnapshot(_config.L2RefreshDepth, seq);
        }
    }

    internal CommandResultCode AcceptBinaryCommand(OrderCommand cmd)
    {
        if (cmd.Command is not OrderCommandType.BINARY_DATA_COMMAND)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        var command = BinaryCommandFactory.GetBinaryCommand(cmd.BinaryCommandType, cmd.BinaryData);

        if (command == null)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        if (command is BatchAddSymbolsCommand addSymbolsCommand)
        {
            _logger.LogDebug("Adding symbols into order books. OrderId={OrderId}", cmd.OrderId);
            return addSymbolsCommand.HandleCommand(_orderBookProvider);
        }

        return cmd.ResultCode;
    }

    internal CommandResultCode AcceptBinaryQuery(OrderCommand cmd)
    {
        if (cmd.Command is not OrderCommandType.BINARY_DATA_QUERY)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        var query = BinaryQueryFactory.GetBinaryQuery(cmd.BinaryCommandType, cmd.BinaryData);

        if (query == null)
        {
            return CommandResultCode.BINARY_COMMAND_FAILED;
        }

        if (query is SingleUserReportQuery singleUserReport)
        {
            _logger.LogDebug("Populate SingleUserReportQuery result. OrderId={OrderId}", cmd.OrderId);
            return singleUserReport.HandleQuery(cmd, _orderBookProvider);
        }

        return CommandResultCode.SUCCESS;
    }

    private bool HasErrorResult(OrderCommand cmd)
    {
        return cmd.ResultCode is
            not CommandResultCode.SUCCESS and
            not CommandResultCode.NEW and
            not CommandResultCode.VALID_FOR_MATCHING_ENGINE;
    }
}
