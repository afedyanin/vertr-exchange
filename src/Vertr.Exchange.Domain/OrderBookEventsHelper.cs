using Vertr.Exchange.Domain.Abstractions;

namespace Vertr.Exchange.Domain;

internal static class OrderBookEventsHelper
{
    public static MatcherTradeEvent CreateTradeEvent(
        IOrder matchingOrder,
        bool makerCompleted,
        bool takerCompleted,
        long size)
    {
        return new MatcherTradeEvent
        {
            EventType = MatcherEventType.TRADE,
            ActiveOrderCompleted = takerCompleted,
            MatchedOrderId = matchingOrder.OrderId,
            MatchedOrderUid = matchingOrder.Uid,
            MatchedOrderCompleted = makerCompleted,
            Price = matchingOrder.Price,
            Size = size,
        };
    }

    public static MatcherTradeEvent CreateReduceEvent(
        IOrder order,
        long reduceSize,
        bool completed)
    {
        return new MatcherTradeEvent
        {
            EventType = MatcherEventType.REDUCE,
            ActiveOrderCompleted = completed,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = order.Price,
            Size = reduceSize,
        };
    }

    public static void AttachRejectEvent(OrderCommand cmd, long rejectedSize)
    {
        cmd.MatcherEvent = new MatcherTradeEvent
        {
            EventType = MatcherEventType.REJECT,
            ActiveOrderCompleted = true,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = cmd.Price,
            Size = rejectedSize,
            NextEvent = cmd.MatcherEvent
        };
    }
}
