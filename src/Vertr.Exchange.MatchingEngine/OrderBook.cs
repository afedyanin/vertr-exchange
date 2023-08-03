using System.Diagnostics;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class OrderBook : IOrderBook
{
    // Key = OrderId
    private readonly IDictionary<long, IOrder> _orders;

    // Key = Price 
    private readonly SortedDictionary<long, OrdersBucket> _bidBuckets;
    private readonly SortedDictionary<long, OrdersBucket> _askBuckets;

    public OrderBook()
    {
        _orders = new Dictionary<long, IOrder>();
        _askBuckets = new SortedDictionary<long, OrdersBucket>();
        _bidBuckets = new SortedDictionary<long, OrdersBucket>(LongDescendingComparer.Instance);
    }

    public IOrder? GetOrder(long orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        return order;
    }

    public bool AddNewOrder(IOrder order)
    {
        if (_orders.ContainsKey(order.OrderId))
        {
            return false;
        }

        _orders.Add(order.OrderId, order);

        return UpdateOrder(order);
    }

    public bool UpdateOrder(IOrder order)
    {
        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.ContainsKey(order.Price))
        {
            buckets.Add(order.Price, new OrdersBucket(order.Price));
        }

        var bucket = buckets[order.Price];
        bucket.Put(order);

        return true;
    }

    public bool RemoveOrder(IOrder order)
    {
        var removedFromOrders = _orders.Remove(order.OrderId);

        if (!removedFromOrders)
        {
            return false;
        }

        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.TryGetValue(order.Price, out var bucket))
        {
            return false;
        }

        var removedFromBucket = bucket.Remove(order);

        if (!removedFromBucket)
        {
            return false;
        }

        if (bucket.TotalVolume == 0L)
        {
            buckets.Remove(order.Price);
        }

        return true;
    }

    public L2MarketData GetL2MarketDataSnapshot(int size)
    {
        var asksSize = GetTotalAskBuckets(size);
        var bidsSize = GetTotalBidBuckets(size);
        var data = new L2MarketData(asksSize, bidsSize);
        FillAsks(asksSize, data);
        FillBids(bidsSize, data);
        return data;
    }

    public long TryMatchInstantly(OrderCommand orderCommand, long filled = 0L)
    {
        var matchingBuckets = GetBucketsForMatching(orderCommand.Action);

        if (!matchingBuckets.Any())
        {
            // no orders to match
            return filled;
        }

        var orderSize = orderCommand.Size;
        var emptyBuckets = new List<long>();

        foreach (var bucket in matchingBuckets.Values)
        {
            if (!CanMatch(orderCommand.Action, orderCommand.Price, bucket.Price))
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
            orderCommand.AttachMatcherEvents(bucketMatchings);

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

    public long Reduce(IOrder order, long requestedReduceSize)
    {
        var reduceBy = Math.Min(order.Remaining, requestedReduceSize);

        order.ReduceSize(reduceBy);

        if (order.Completed)
        {
            RemoveOrder(order);
        }
        else
        {
            ReduceBucketSize(order, reduceBy);
        }

        return reduceBy;
    }


    private SortedDictionary<long, OrdersBucket> GetBucketsByAction(OrderAction action)
        => action == OrderAction.ASK ? _askBuckets : _bidBuckets;

    private SortedDictionary<long, OrdersBucket> GetBucketsForMatching(OrderAction action)
        => action == OrderAction.ASK ? _bidBuckets : _askBuckets;

    private bool CanMatch(OrderAction action, long orderPrice, long bucketPrice)
    {
        if (action == OrderAction.BID && bucketPrice > orderPrice)
        {
            // цена оффера в бакете выше, чем цена покупки
            return false;
        }

        if (action == OrderAction.ASK && bucketPrice < orderPrice)
        {
            // цена покупки в бакете меньше, цем цена предложения
            return false;
        }

        return true;
    }

    private int GetTotalAskBuckets(int limit)
        => Math.Min(limit, _askBuckets.Count);

    private int GetTotalBidBuckets(int limit)
        => Math.Min(limit, _bidBuckets.Count);

    private void FillAsks(int size, L2MarketData data)
    {
        if (size == 0)
        {
            data.AskSize = 0;
            return;
        }

        var i = 0;

        foreach (var bucket in _askBuckets.Values)
        {
            data.AskPrices[i] = bucket.Price;
            data.AskVolumes[i] = bucket.TotalVolume;
            data.AskOrders[i] = bucket.OrdersCount;

            if (++i == size)
            {
                break;
            }
        }

        data.AskSize = i;
    }

    private void FillBids(int size, L2MarketData data)
    {
        if (size == 0)
        {
            data.BidSize = 0;
            return;
        }

        var i = 0;

        foreach (var bucket in _bidBuckets.Values)
        {
            data.BidPrices[i] = bucket.Price;
            data.BidVolumes[i] = bucket.TotalVolume;
            data.BidOrders[i] = bucket.OrdersCount;
            if (++i == size)
            {
                break;
            }
        }
        data.BidSize = i;
    }

    internal bool ValidateInternalState()
    {
        var asksIsValid = _askBuckets.Values.All(b => b.IsValid());
        var bidsIsValid = _bidBuckets.Values.All(b => b.IsValid());

        return asksIsValid && bidsIsValid;
    }

    private void ReduceBucketSize(IOrder order, long reduceBy)
    {
        var bucket = GetBucketsByAction(order.Action)[order.Price];
        Debug.Assert(bucket != null);
        bucket.ReduceSize(reduceBy);
    }
}
