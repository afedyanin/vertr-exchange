using System.Diagnostics;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine.OrderBooks;

internal sealed class OrderBook : IOrderBook
{
    // Key = OrderId
    private readonly Dictionary<long, IOrder> _orders;

    // Key = Price 
    private readonly SortedDictionary<decimal, OrdersBucket> _bidBuckets;
    private readonly SortedDictionary<decimal, OrdersBucket> _askBuckets;

    public OrderBook()
    {
        _orders = [];
        _askBuckets = [];
        _bidBuckets = new SortedDictionary<decimal, OrdersBucket>(DecimalDescendingComparer.Instance);
    }

    public IOrder? GetOrder(long orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        return order;
    }

    public IOrder[] GetOrdersByUid(long uid)
    {
        return _orders.Values.Where(ord => ord.Uid == uid).ToArray();
    }

    public bool AddOrder(IOrder order)
    {
        if (_orders.ContainsKey(order.OrderId))
        {
            return false;
        }

        _orders.Add(order.OrderId, order);

        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.TryGetValue(order.Price, out var value))
        {
            value = new OrdersBucket(order.Price);
            buckets.Add(order.Price, value);
        }

        var bucket = value;
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

    public L2MarketData GetL2MarketDataSnapshot(int size, long seq)
    {
        var asksSize = GetTotalAskBuckets(size);
        var bidsSize = GetTotalBidBuckets(size);
        var data = new L2MarketData(asksSize, bidsSize, seq);
        FillAsks(asksSize, data);
        FillBids(bidsSize, data);
        return data;
    }

    public MatcherResult TryMatchInstantly(
        OrderAction action,
        decimal price,
        long size,
        long filled = 0L)
    {
        var matchingBuckets = GetBucketsForMatching(action);

        if (matchingBuckets.Count == 0)
        {
            // no orders to match
            return new MatcherResult(filled);
        }

        var emptyBucketIds = new List<decimal>();
        var filledOrderIds = new List<long>();
        var matcherEvents = new List<IEngineEvent>();

        foreach (var bucket in matchingBuckets.Values)
        {
            if (!CanMatch(action, price, bucket.Price))
            {
                break;
            }

            if (filled == size)
            {
                break;
            }

            var sizeLeft = size - filled;
            var bucketMatchings = bucket.Match(sizeLeft);

            filledOrderIds.AddRange(bucketMatchings.OrdersToRemove);

            if (bucket.TotalVolume == 0L)
            {
                emptyBucketIds.Add(bucket.Price);
            }

            matcherEvents.AddRange(bucketMatchings.TradeEvents);
            filled += bucketMatchings.Volume;
        }

        RemoveFilledOrders(filledOrderIds);
        RemoveEmptyBuckets(matchingBuckets, emptyBucketIds);

        return new MatcherResult(filled, matcherEvents);
    }

    public long Reduce(IOrder order, long requestedReduceSize)
    {
        if (requestedReduceSize < 0L)
        {
            throw new ArgumentOutOfRangeException(nameof(requestedReduceSize), "Requested reduce size cannot be < 0.");
        }

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

    private void RemoveEmptyBuckets(SortedDictionary<decimal, OrdersBucket> backets, IEnumerable<decimal> ids)
    {
        foreach (var id in ids)
        {
            backets.Remove(id);
        }
    }

    private SortedDictionary<decimal, OrdersBucket> GetBucketsByAction(OrderAction action)
        => action == OrderAction.ASK ? _askBuckets : _bidBuckets;

    private SortedDictionary<decimal, OrdersBucket> GetBucketsForMatching(OrderAction action)
        => action == OrderAction.ASK ? _bidBuckets : _askBuckets;

    private static bool CanMatch(OrderAction action, decimal price, decimal bucketPrice)
    {
        if (action == OrderAction.BID && bucketPrice > price)
        {
            // продажная цена выше, чем запрашиваемая цена покупки
            return false;
        }

        if (action == OrderAction.ASK && bucketPrice < price)
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

        data.AskPrices = new decimal[size];
        data.AskVolumes = new long[size];
        data.AskOrders = new long[size];

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

        data.BidPrices = new decimal[size];
        data.BidVolumes = new long[size];
        data.BidOrders = new long[size];

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
