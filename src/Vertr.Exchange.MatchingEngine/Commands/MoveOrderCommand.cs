using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class MoveOrderCommand : OrderBookCommand
{
    public MoveOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var orderId = OrderCommand.OrderId;
        var newPrice = OrderCommand.Price;

        var order = OrderBook.GetOrder(orderId);

        if (order is null)
        {
            // already matched, moved or cancelled
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        if (order.Uid != OrderCommand.Uid)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        OrderBook.RemoveOrder(order);

        // fill action fields (for events handling)
        OrderCommand.Action = order.Action;

        order.Price = newPrice;

        // try match with new price
        var filled = OrderBook.TryMatchInstantly(order, order.Filled, OrderCommand);

        if (filled == order.Size)
        {
            OrderBook.RemoveOrder(order);
            return CommandResultCode.SUCCESS;
        }

        order.Filled = filled;

        // if not filled completely - put it into corresponding bucket
        OrderBook.UpdateOrder(order);
        return CommandResultCode.SUCCESS;

    }
}
