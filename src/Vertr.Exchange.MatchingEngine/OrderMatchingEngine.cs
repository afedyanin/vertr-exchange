using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;

[assembly: InternalsVisibleTo("Vertr.Exchange.MatchingEngine.Tests")]

namespace Vertr.Exchange.MatchingEngine;

public class OrderMatchingEngine : IOrderMatchingEngine
{
    private readonly MatchingEngineConfiguration _config;

    // Key = Symbol
    private readonly IDictionary<int, IOrderBook> _orderBooks;

    public OrderMatchingEngine(
        IOptions<MatchingEngineConfiguration> options,
        )
    {
        _config = options.Value;
        _orderBooks = new Dictionary<int, IOrderBook>();
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
                ProcessMatchingCommand(cmd);
                break;
            case OrderCommandType.RESET:
                _orderBooks.Clear();
                cmd.ResultCode = CommandResultCode.SUCCESS;
                break;
            case OrderCommandType.NOP:
                cmd.ResultCode = CommandResultCode.SUCCESS;
                break;
            case OrderCommandType.BINARY_DATA_COMMAND:
                cmd.ResultCode = AcceptBinaryCommand(cmd);
                break;
            case OrderCommandType.BINARY_DATA_QUERY:
            case OrderCommandType.ADD_USER:
            case OrderCommandType.BALANCE_ADJUSTMENT:
            case OrderCommandType.SUSPEND_USER:
            case OrderCommandType.RESUME_USER:
            case OrderCommandType.PERSIST_STATE_MATCHING:
            case OrderCommandType.PERSIST_STATE_RISK:
            case OrderCommandType.GROUPING_CONTROL:
            case OrderCommandType.SHUTDOWN_SIGNAL:
            case OrderCommandType.RESERVED_COMPRESSED:
                break;
            default:
                // TODO: How to handle exception here
                throw new NotSupportedException(cmd.Command.ToString());
        }
    }

    internal void ProcessMatchingCommand(OrderCommand cmd)
    {
        if (!_orderBooks.TryGetValue(cmd.Symbol, out var orderBook))
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
        if (cmd.Command is OrderCommandType.BINARY_DATA_COMMAND)
        {
            var command = BinaryCommandFactory.GetBinaryCommand(cmd.BinaryCommandType, cmd.BinaryData);

            if (command != null)
            {
                return HandleBinaryCommand(command);
            }
        }

        return CommandResultCode.BINARY_COMMAND_FAILED;
    }

    private CommandResultCode HandleBinaryCommand(IBinaryCommand binCmd)
    {
        if (binCmd is BatchAddSymbolsCommand symCmd)
        {
            foreach (var sym in symCmd.Symbols)
            {
                AddSymbol(sym.SymbolId);
            }
        }

        return CommandResultCode.SUCCESS;
    }

    private void AddSymbol(int symbol)
    {
        if (!_orderBooks.ContainsKey(symbol))
        {
            _orderBooks.Add(symbol, new OrderBook());
        }
        else
        {
            // Warn: symbol already added.
        }
    }
}
