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
            EventType = MatcherEventType.Trade,
            ActiveOrderCompleted = takerCompleted,
            MatchedOrderId = matchingOrder.OrderId,
            MatchedOrderUid = matchingOrder.Uid,
            MatchedOrderCompleted = makerCompleted,
            Price = matchingOrder.Price,
            Size = size,
        };
    }
}
