using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands.NewOrder;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal static class OrderBookCommandFactory
{
    public static OrderBookCommand CreateOrderBookCommand(OrderBook orderBook, OrderCommand cmd)
    {
        var commandType = cmd.Command;

        if (commandType == OrderCommandType.MOVE_ORDER)
        {
            return new MoveOrderCommand(orderBook, cmd);
        }
        else if (commandType == OrderCommandType.CANCEL_ORDER)
        {
            return new CancelOrderCommand(orderBook, cmd);
        }
        else if (commandType == OrderCommandType.REDUCE_ORDER)
        {
            return new ReduceOrderCommand(orderBook, cmd);
        }
        else if (commandType == OrderCommandType.PLACE_ORDER)
        {
            if (cmd.ResultCode == CommandResultCode.VALID_FOR_MATCHING_ENGINE)
            {
                return CreateNewOrderBookCommand(orderBook, cmd);
            }
        }
        else if (commandType == OrderCommandType.ORDER_BOOK_REQUEST)
        {
            return new MarketDataSnapshotCommand(orderBook, cmd);
        }
        else
        {
            return new MatchingUnsupportedCommand(orderBook, cmd);
        }

        return new NoChangeOrderCommand(orderBook, cmd);
    }

    private static OrderBookCommand CreateNewOrderBookCommand(OrderBook orderBook, OrderCommand cmd)
    {
        return cmd.OrderType switch
        {
            OrderType.GTC => new NewGtcOrderCommand(orderBook, cmd),
            OrderType.IOC => new NewIocOrderCommand(orderBook, cmd),
            _ => new RejectOrderCommand(orderBook, cmd),
        };
    }
}
