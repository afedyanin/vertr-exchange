using System.Diagnostics;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.MatchingEngine.Abstractions;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class OrdersBucket
{
    private readonly IMatcherTradeEventFactory _eventFactory;
    private readonly LinkedList<IOrder> _orders;

    public long Price { get; }

    public long TotalVolume { get; private set; }

    public int OrdersCount => _orders.Count;

    public OrdersBucket(
        IMatcherTradeEventFactory eventFactory,
        long price)
    {
        Debug.Assert(price >= 0L);
        Price = price;
        TotalVolume = 0L;
        _orders = new LinkedList<IOrder>();
        _eventFactory = eventFactory;
    }

    public IOrder? FindOrder(long orderId)
        => _orders.FirstOrDefault(o => o.OrderId == orderId);

    public bool IsValid()
        => _orders.Sum(o => o.Remaining) == TotalVolume;

    public void Put(IOrder order)
    {
        Debug.Assert(order != null);
        Debug.Assert(order.Price == Price);

        // remove if exists
        Remove(order);

        _orders.AddLast(order);
        TotalVolume += order.Remaining;
    }

    public bool Remove(IOrder order)
    {
        Debug.Assert(order != null);

        var removed = _orders.Remove(order);

        if (removed)
        {
            TotalVolume -= order.Remaining;
        }

        return removed;
    }

    public void ReduceSize(long reduceSize)
    {
        TotalVolume -= reduceSize;
    }

    public MatcherResult Match(long volumeToCollect)
    {
        var totalMatchingVolume = 0L;
        var ordersToRemove = new List<long>();
        var tradeEvents = new LinkedList<IMatcherTradeEvent>();

        var node = _orders.First;

        while (node != null && volumeToCollect > 0L)
        {
            var order = node.Value;

            var volume = Math.Min(volumeToCollect, order.Remaining);

            totalMatchingVolume += volume;
            // !!! order.Filled += volume;

            volumeToCollect -= volume;
            TotalVolume -= volume;

            var fullMatch = order.Size == order.Filled;

            var tradeEvent = _eventFactory.CreateTradeEvent(order, fullMatch, volumeToCollect == 0L, volume);
            tradeEvents.AddLast(tradeEvent!);

            if (fullMatch)
            {
                ordersToRemove.Add(order.OrderId);
            }

            node = node.Next;
        }

        return new MatcherResult(totalMatchingVolume, ordersToRemove.ToArray(), tradeEvents.ToArray());
    }
}
