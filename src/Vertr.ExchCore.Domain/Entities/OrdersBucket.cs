using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.Events.TradeEvents;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Entities;

public class OrdersBucket
{
    private readonly IDictionary<long, Order> _entries;

    public long Price { get; set; }

    public long TotalVolume { get; private set; }

    public OrdersBucket(long price)
    {
        Price = price;
        TotalVolume = 0;
        _entries = new Dictionary<long, Order>();
    }

    public void Add(Order order)
    {
        _entries.Add(order.OrderId, order);
        TotalVolume += order.Size - order.Filled;
    }

    public Order? Remove(long orderId, long uid)
    {
        _entries.TryGetValue(orderId, out Order? order);
        if (order == null)
        {
            return null;
        }

        TotalVolume -= order.Size - order.Filled;
        return order;
    }

    public MatcherResult Match(long volumeToCollect, IOrder activeOrder, OrderBookEventsHelper helper)
    {
        long totalMatchingVolume = 0;
        var ordersToRemove = new List<long>();

        MatcherTradeEvent? eventsHead = null;
        MatcherTradeEvent? eventsTail = null;

        foreach(var order in _entries.Values)
        {
            if (volumeToCollect <= 0)
            {
                break;
            }

            var v = Math.Min(volumeToCollect, order.Size - order.Filled);
            totalMatchingVolume += v;

            order.Filled += v;
            volumeToCollect -= v;
            TotalVolume -= v;

            var fullMatch = order.Size == order.Filled;

            var bidderHoldPrice = order.Action == OrderAction.ASK ? activeOrder.ReserveBidPrice : order.ReserveBidPrice;
            var tradeEvent = helper.SendTradeEvent(order, fullMatch, volumeToCollect == 0, v, bidderHoldPrice);

            if (eventsTail == null)
            {
                eventsHead = tradeEvent;
            }
            else
            {
                eventsTail.NextEvent = tradeEvent;
            }

            eventsTail = tradeEvent;

            if (fullMatch)
            {
                ordersToRemove.Add(order.OrderId);
            }
        }

        foreach(var orderId in ordersToRemove)
        {
            _entries.Remove(orderId);
        }

        return new MatcherResult(eventsHead!, eventsTail!, totalMatchingVolume, ordersToRemove);
    }
}
