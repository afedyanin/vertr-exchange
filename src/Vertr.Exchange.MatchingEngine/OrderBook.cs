using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Commands;
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

    public CommandResultCode ProcessCommand(OrderCommand cmd)
    {
        var orderBookCommand = OrderBookCommandFactory.CreateOrderBookCommand(this, cmd);
        return orderBookCommand.Execute();
    }

    public IOrder? GetOrder(long orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        return order;
    }

    public void RemoveOrder(long orderId)
    {
        _orders.Remove(orderId);
    }

    public OrdersBucket GetBucket(IOrder order)
    {
        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.TryGetValue(order.Price, out var bucket))
        {
            throw new InvalidOperationException($"Can not find bucket for OrderId={order.OrderId} Price={order.Price}");
        }

        return bucket;
    }

    public void RemoveBucket(IOrder order)
    {
        var buckets = GetBucketsByAction(order.Action);
        buckets.Remove(order.Price);
    }

    private SortedDictionary<long, OrdersBucket> GetBucketsByAction(OrderAction action)
        => action == OrderAction.ASK ? _askBuckets : _bidBuckets;

    internal bool ValidateInternalState()
    {
        var asksIsValid = _askBuckets.Values.All(b => b.IsValid());
        var bidsIsValid = _bidBuckets.Values.All(b => b.IsValid());

        return asksIsValid && bidsIsValid;
    }
}
