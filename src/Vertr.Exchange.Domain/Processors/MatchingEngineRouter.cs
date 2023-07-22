using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Binary;
using Vertr.Exchange.Domain.Enums;
using Vertr.Exchange.Domain.MatchingEngine;

namespace Vertr.Exchange.Domain.Processors;
public class MatchingEngineRouter
{
    private static readonly int _cfgL2RefreshDepth = int.MaxValue;

    // Key = Symbol
    private readonly IDictionary<int, IOrderBook> _orderBooks;

    private readonly BinaryCommandProcessor _binaryCommandsProcessor;

    public MatchingEngineRouter()
    {
        _orderBooks = new Dictionary<int, IOrderBook>();

        _binaryCommandsProcessor = new BinaryCommandProcessor(HandleBinaryCommand);
    }

    public void ProcessOrderCommand(OrderCommand cmd)
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
                cmd.ResultCode = _binaryCommandsProcessor.AcceptBinaryCommand(cmd);
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
                throw new NotSupportedException(cmd.Command.ToString());
        }
    }

    public void ProcessMatchingCommand(OrderCommand cmd)
    {
        if (!_orderBooks.TryGetValue(cmd.Symbol, out var orderBook))
        {
            cmd.ResultCode = CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID;
            return;
        }

        cmd.ResultCode = orderBook.ProcessCommand(cmd);

        if (cmd.Command != OrderCommandType.ORDER_BOOK_REQUEST
            && cmd.ResultCode == CommandResultCode.SUCCESS)
        {
            cmd.MarketData = orderBook.GetL2MarketDataSnapshot(_cfgL2RefreshDepth);
        }
    }

    private void HandleBinaryCommand(OrderCommand cmd, BinaryCommand binCmd)
    {
        if (binCmd is BatchAddSymbolsCommand symCmd)
        {
            foreach (var sym in symCmd.Symbols)
            {
                AddSymbol(sym);
            }
        }
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
