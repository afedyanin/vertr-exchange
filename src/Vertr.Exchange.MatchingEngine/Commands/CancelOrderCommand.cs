using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class CancelOrderCommand : OrderBookCommand
{
    public CancelOrderCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var orderId = OrderCommand.OrderId;

        if (!_orders.TryGetValue(orderId, out var order))
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        if (order.Uid != cmd.Uid)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        _orders.Remove(orderId);

        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.TryGetValue(order.Price, out var bucket))
        {
            throw new InvalidOperationException($"Can not find bucket for OrderId={order.OrderId} Price={order.Price}");
        }

        var removed = bucket.Remove(order);

        if (!removed)
        {
            // TODO: How to handle fail removing
            throw new InvalidOperationException($"Can not remove OrderId={order.OrderId}");
        }

        if (bucket.TotalVolume == 0L)
        {
            buckets.Remove(order.Price);
        }

        OrderCommand.MatcherEvent = _eventFactory.CreateReduceEvent(order, order.Remaining, true);

        // fill action fields (for events handling)
        // TODO: How and where is it used?
        OrderCommand.Action = order.Action;

        return CommandResultCode.SUCCESS;
    }
}
