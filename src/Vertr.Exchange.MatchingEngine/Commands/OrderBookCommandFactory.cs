using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands.NewOrder;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal static class OrderBookCommandFactory
{
    public static OrderBookCommand CreateOrderBookCommand(
        IOrderBook orderBook,
        OrderCommand cmd,
        long sequence)
    {
        switch (cmd.Command)
        {
            case OrderCommandType.PLACE_ORDER:
                if (cmd.ResultCode == CommandResultCode.VALID_FOR_MATCHING_ENGINE)
                {
                    return CreateNewOrderBookCommand(orderBook, cmd);
                }
                return new MatchingUnsupportedCommand(orderBook, cmd);
            case OrderCommandType.REDUCE_ORDER:
                return new ReduceOrderCommand(orderBook, cmd);
            case OrderCommandType.CANCEL_ORDER:
                return new CancelOrderCommand(orderBook, cmd);
            case OrderCommandType.MOVE_ORDER:
                return new MoveOrderCommand(orderBook, cmd);
            case OrderCommandType.ORDER_BOOK_REQUEST:
                return new MarketDataSnapshotCommand(orderBook, cmd, sequence);
            case OrderCommandType.ADD_USER:
            case OrderCommandType.BALANCE_ADJUSTMENT:
            case OrderCommandType.SUSPEND_USER:
            case OrderCommandType.RESUME_USER:
            case OrderCommandType.BINARY_DATA_QUERY:
            case OrderCommandType.BINARY_DATA_COMMAND:
            case OrderCommandType.PERSIST_STATE_MATCHING:
            case OrderCommandType.PERSIST_STATE_RISK:
            case OrderCommandType.GROUPING_CONTROL:
            case OrderCommandType.NOP:
            case OrderCommandType.RESET:
            case OrderCommandType.SHUTDOWN_SIGNAL:
            case OrderCommandType.RESERVED_COMPRESSED:
            default:
                return new MatchingUnsupportedCommand(orderBook, cmd);
        }
    }

    private static OrderBookCommand CreateNewOrderBookCommand(IOrderBook orderBook, OrderCommand cmd)
    {
        return cmd.OrderType switch
        {
            OrderType.GTC => new NewGtcOrderCommand(orderBook, cmd),
            OrderType.IOC => new NewIocOrderCommand(orderBook, cmd),
            OrderType.IOC_BUDGET => new RejectOrderCommand(orderBook, cmd),
            OrderType.FOK => new RejectOrderCommand(orderBook, cmd),
            OrderType.FOK_BUDGET => new RejectOrderCommand(orderBook, cmd),
            _ => new RejectOrderCommand(orderBook, cmd),
        };
    }
}
