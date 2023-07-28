using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class ReduceOrderCommand : OrderBookCommand
{
    public ReduceOrderCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
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
        var bucket = OrderBook.GetBucket(order);

        var canRemove = reduceBy == remainingSize;

        if (canRemove)
        {
            // now can remove order
            OrderBook.RemoveOrder(orderId);

            // canRemove order and whole bucket if it is empty
            bucket.Remove(order);

            var removed = bucket.Remove(order);

            if (!removed)
            {
                // TODO: How to handle fail removing
                throw new InvalidOperationException($"Can not remove OrderId={order.OrderId}");
            }

            if (bucket.TotalVolume == 0L)
            {
                OrderBook.RemoveBucket(order);
            }
        }
        else
        {
            order.Size -= reduceBy;
            bucket.ReduceSize(reduceBy);
        }

        // send reduce event
        OrderCommand.MatcherEvent = _eventFactory.CreateReduceEvent(order, reduceBy, canRemove);

        // fill action fields (for events handling)
        OrderCommand.Action = order.Action;

        return CommandResultCode.SUCCESS;
    }
}
