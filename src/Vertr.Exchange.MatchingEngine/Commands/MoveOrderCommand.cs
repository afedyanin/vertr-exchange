using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class MoveOrderCommand : OrderBookCommand
{
    public MoveOrderCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
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

        var bucket = OrderBook.GetBucket(order);

        // fill action fields (for events handling)
        OrderCommand.Action = order.Action;

        // take order out of the original bucket and clean bucket if its empty
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

        order.Price = newPrice;

        // try match with new price
        var filled = TryMatchInstantly(order, order.Filled, cmd);

        if (filled == order.Size)
        {
            // order was fully matched (100% marketable) - removing from order book
            OrderBook.RemoveOrder(orderId);
            return CommandResultCode.SUCCESS;
        }

        order.Filled = filled;

        // if not filled completely - put it into corresponding bucket

        if (!buckets.ContainsKey(order.Price))
        {
            buckets.Add(order.Price, new OrdersBucket(_eventFactory, order.Price));
        }

        var anotherBucket = buckets[order.Price];
        anotherBucket.Put(order);

        return CommandResultCode.SUCCESS;

    }
}
