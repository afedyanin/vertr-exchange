using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Helpers;

internal static class OrderCommandEventsHelper
{
    public static void AttachMatcherEvents(this OrderCommand command, MatcherResult matcherResult)
    {
        MatcherTradeEvent? eventsTail = null;

        foreach (var evt in matcherResult.TradeEvents)
        {
            if (eventsTail == null)
            {
                command.MatcherEvent = evt;
            }
            else
            {
                eventsTail.NextEvent = evt;
            }

            eventsTail = evt;
        }
    }

    public static void AttachReduceEvent(
        this OrderCommand command,
        IOrder order,
        long reduceSize,
        bool completed)
    {
        var evt = new MatcherTradeEvent
        {
            EventType = MatcherEventType.REDUCE,
            ActiveOrderCompleted = completed,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = order.Price,
            Size = reduceSize,
        };

        command.MatcherEvent = evt;
    }

    public static void AttachRejectEvent(
        this OrderCommand command,
        decimal price,
        long rejectedSize)
    {
        var evt = new MatcherTradeEvent
        {
            EventType = MatcherEventType.REJECT,
            ActiveOrderCompleted = true,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = price,
            Size = rejectedSize,
        };

        command.MatcherEvent = evt;
    }
}
