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

        var emptyBucketIds = new List<long>();
        var filledOrderIds = new List<long>();

        foreach (var bucket in matchingBuckets.Values)
        {
            if (!CanMatch(orderCommand, bucket.Price))
            {
                break;
            }

            if (filled == orderCommand.Size)
            {
                break;
            }

            var sizeLeft = orderCommand.Size - filled;
            var bucketMatchings = bucket.Match(sizeLeft);

            filledOrderIds.AddRange(bucketMatchings.OrdersToRemove);

            if (bucket.TotalVolume == 0L)
            {
                emptyBucketIds.Add(bucket.Price);
            }

            orderCommand.AttachMatcherEvents(bucketMatchings);
            filled += bucketMatchings.Volume;
        }

        RemoveFilledOrders(filledOrderIds);
        RemoveEmptyBuckets(matchingBuckets, emptyBucketIds);

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

    private void RemoveFilledOrders(IEnumerable<long> orderIds)
    {
        foreach (var orderId in orderIds)
        {
            _orders.Remove(orderId);
        }
    }

    private void RemoveEmptyBuckets(SortedDictionary<long, OrdersBucket> backets, IEnumerable<long> ids)
    {
        foreach (var id in ids)
        {
            backets.Remove(id);
        }
    }

    private SortedDictionary<long, OrdersBucket> GetBucketsByAction(OrderAction action)
        => action == OrderAction.ASK ? _askBuckets : _bidBuckets;

    private SortedDictionary<long, OrdersBucket> GetBucketsForMatching(OrderAction action)
        => action == OrderAction.ASK ? _bidBuckets : _askBuckets;

    private static bool CanMatch(OrderCommand command, long bucketPrice)
    {
        if (command.Action == OrderAction.BID && bucketPrice > command.Price)
        {
            // продажная цена выше, чем запрашиваемая цена покупки
            return false;
        }

        if (command.Action == OrderAction.ASK && bucketPrice < command.Price)
        {
            // покупная цена меньше, чем запрашиваемая цена продажи
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
