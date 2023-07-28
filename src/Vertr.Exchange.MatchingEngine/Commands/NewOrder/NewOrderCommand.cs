using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal abstract class NewOrderCommand : OrderBookCommand
{
    protected NewOrderCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    protected long TryMatchInstantly(IOrder activeOrder, long filled, OrderCommand triggerCmd)
    {
        var matchingBuckets = activeOrder.Action == OrderAction.ASK ? _bidBuckets : _askBuckets;

        if (!matchingBuckets.Any())
        {
            return filled;
        }

        var orderSize = activeOrder.Size;
        var emptyBuckets = new List<long>();
        MatcherTradeEvent? eventsTail = null;

        foreach (var bucket in matchingBuckets.Values)
        {
            if (activeOrder.Action == OrderAction.BID && bucket.Price > activeOrder.Price)
            {
                break;
            }

            if (activeOrder.Action == OrderAction.ASK && bucket.Price < activeOrder.Price)
            {
                break;
            }

            var sizeLeft = orderSize - filled;
            var bucketMatchings = bucket.Match(sizeLeft);

            foreach (var orderId in bucketMatchings.OrdersToRemove)
            {
                _orders.Remove(orderId);
            }

            filled += bucketMatchings.Volume;

            // attach chain received from bucket matcher
            foreach (var evt in bucketMatchings.TradeEvents)
            {
                if (eventsTail == null)
                {
                    triggerCmd.MatcherEvent = evt;
                }
                else
                {
                    eventsTail.NextEvent = evt;
                }

                eventsTail = evt;
            }

            var price = bucket.Price;

            // remove empty buckets
            if (bucket.TotalVolume == 0L)
            {
                emptyBuckets.Add(price);
            }

            if (filled == orderSize)
            {
                // enough matched
                break;
            }
        }

        foreach (var key in emptyBuckets)
        {
            matchingBuckets.Remove(key);
        }

        return filled;
    }

}
