using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Abstractions;

namespace Vertr.Exchange.MatchingEngine.Events;
internal sealed class MatcherTradeEventFactory : IMatcherTradeEventFactory
{
    public IMatcherTradeEvent CreateTradeEvent(
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

    public IMatcherTradeEvent CreateReduceEvent(
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

    public IMatcherTradeEvent CreateRejectEvent(
        long price,
        long rejectedSize)
    {
        return new MatcherTradeEvent
        {
            EventType = MatcherEventType.REJECT,
            ActiveOrderCompleted = true,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = price,
            Size = rejectedSize,
        };
    }

}
