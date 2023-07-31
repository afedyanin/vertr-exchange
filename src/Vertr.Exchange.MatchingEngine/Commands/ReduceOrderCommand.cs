using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class ReduceOrderCommand : OrderBookCommand
{
    public ReduceOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var orderId = OrderCommand.OrderId;
        var requestedReduceSize = OrderCommand.Size;

        if (requestedReduceSize <= 0)
        {
            return CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE;
        }

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

        var remainingSize = order.Remaining;
        var reduceBy = Math.Min(remainingSize, requestedReduceSize);
        var canRemove = reduceBy == remainingSize;

        if (canRemove)
        {
            OrderBook.RemoveOrder(order);
        }
        else
        {
            order.Size -= reduceBy;
            bucket.ReduceSize(reduceBy);
        }

        // send reduce event
        OrderCommand.MatcherEvent = CreateReduceEvent(order, reduceBy, canRemove);

        // fill action fields (for events handling)
        OrderCommand.Action = order.Action;

        return CommandResultCode.SUCCESS;
    }
}
