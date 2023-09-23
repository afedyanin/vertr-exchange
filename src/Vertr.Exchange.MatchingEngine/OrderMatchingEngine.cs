using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary.Commands;
using Vertr.Exchange.Common.Binary.Reports;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Binary;
using Vertr.Exchange.MatchingEngine.Commands;
using Vertr.Exchange.MatchingEngine.OrderBooks;

[assembly: InternalsVisibleTo("Vertr.Exchange.MatchingEngine.Tests")]

namespace Vertr.Exchange.MatchingEngine;

public class OrderMatchingEngine : IOrderMatchingEngine
{
    private readonly MatchingEngineConfiguration _config;

    private readonly IOrderBookProvider _orderBookProvider;
    private readonly ILogger<OrderMatchingEngine> _logger;

    public OrderMatchingEngine(
        IOrderBookProvider orderBookProvider,
        IOptions<MatchingEngineConfiguration> options,
        ILogger<OrderMatchingEngine> logger)
    {
        _config = options.Value;
        _orderBookProvider = orderBookProvider;
        _logger = logger;
    }

    public void ProcessOrder(long seq, OrderCommand cmd)
    {
        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
            case OrderCommandType.REDUCE_ORDER:
            case OrderCommandType.CANCEL_ORDER:
            case OrderCommandType.MOVE_ORDER:
            case OrderCommandType.ORDER_BOOK_REQUEST:
                _logger.LogDebug("Processing command={CommandType} OrderId={OrderId} Uid={Uid}", cmd.Command, cmd.OrderId, cmd.Uid);
                ProcessMatchingCommand(cmd);
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

    internal void ProcessMatchingCommand(OrderCommand cmd)
    {
        if (HasErrorResult(cmd))
        {
            return;
        }

        var orderBook = _orderBookProvider.GetOrderBook(cmd.Symbol);

        if (orderBook == null)
        {
            cmd.ResultCode = CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID;
            return;
        }

        var orderBookCommand = OrderBookCommandFactory.CreateOrderBookCommand(orderBook, cmd);

        // TODO: Catch and handle exceptions
        cmd.ResultCode = orderBookCommand.Execute();

        if (cmd.Command != OrderCommandType.ORDER_BOOK_REQUEST
            && cmd.ResultCode == CommandResultCode.SUCCESS)
        {
            cmd.MarketData = orderBook.GetL2MarketDataSnapshot(_config.L2RefreshDepth);
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

        // after risk engine
        if (HasErrorResult(cmd))
        {
            return cmd.ResultCode;
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

        // after risk engine
        if (HasErrorResult(cmd))
        {
            return cmd.ResultCode;
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
