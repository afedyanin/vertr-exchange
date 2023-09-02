using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.MatchingEngine.OrderBooks;

internal sealed class OrdersBucket
{
    private readonly LinkedList<IOrder> _orders;

    public decimal Price { get; }

    public long TotalVolume { get; private set; }

    public int OrdersCount => _orders.Count;

    public OrdersBucket(decimal price)
    {
        if (price < decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price value cannot be < 0.");
        }

        Price = price;
        TotalVolume = 0L;
        _orders = new LinkedList<IOrder>();
    }

    public IOrder? FindOrder(long orderId)
        => _orders.FirstOrDefault(o => o.OrderId == orderId);

    public bool IsValid()
        => _orders.Sum(o => o.Remaining) == TotalVolume;

    public void Put(IOrder order)
    {
        ArgumentNullException.ThrowIfNull(nameof(order));

        if (!order.Price.Equals(Price))
        {
            throw new InvalidOperationException($"Cannot put Order with Price={order.Price} into Bucket with Price={Price}.");
        }

        // remove if exists
        Remove(order);

        _orders.AddLast(order);
        TotalVolume += order.Remaining;
    }

    public bool Remove(IOrder order)
    {
        ArgumentNullException.ThrowIfNull(nameof(order));

        var removed = _orders.Remove(order);

        if (removed)
        {
            TotalVolume -= order.Remaining;
        }

        return removed;
    }

    public void ReduceSize(long reduceSize)
    {
        if (reduceSize < decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(reduceSize), "ReduceSize cannot be < 0.");
        }

        if (TotalVolume < reduceSize)
        {
            throw new InvalidOperationException($"ReduceSize={reduceSize} cannot exceed TotalVolume={TotalVolume}.");
        }

        TotalVolume -= reduceSize;
    }

    public BucketMatcherResult Match(long volumeToCollect)
    {
        var totalMatchingVolume = 0L;
        var ordersToRemove = new List<long>();
        var tradeEvents = new LinkedList<IEngineEvent>();

        var node = _orders.First;

        while (node != null && volumeToCollect > 0L)
        {
            var order = node.Value;

            var volume = Math.Min(volumeToCollect, order.Remaining);

            totalMatchingVolume += volume;
            order.Fill(volume);

            volumeToCollect -= volume;
            TotalVolume -= volume;

            var fullMatch = order.Size == order.Filled;

            var tradeEvent = CreateTradeEvent(order, fullMatch, volumeToCollect == 0L, volume);

            tradeEvents.AddLast(tradeEvent!);

            if (fullMatch)
            {
                ordersToRemove.Add(order.OrderId);
            }

            node = node.Next;
        }

        return new BucketMatcherResult(totalMatchingVolume, ordersToRemove.ToArray(), tradeEvents.ToArray());
    }

    private IEngineEvent CreateTradeEvent(
        IOrder matchingOrder,
        bool makerCompleted,
        bool takerCompleted,
        long size)
    {
        return new EngineEvent
        {
            EventType = EngineEventType.TRADE,
            ActiveOrderCompleted = takerCompleted,
            MatchedOrderId = matchingOrder.OrderId,
            MatchedOrderUid = matchingOrder.Uid,
            MatchedOrderCompleted = makerCompleted,
            Price = matchingOrder.Price,
            Size = size,
        };
    }
}
