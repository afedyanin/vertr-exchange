using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Helpers;

internal static class OrderCommandEventsHelper
{
    public static void AttachMatcherEvents(this OrderCommand command, IEnumerable<IMatcherTradeEvent> tradeEvents)
    {
        IMatcherTradeEvent? eventsTail = null;

        foreach (var evt in tradeEvents)
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
            BidderHoldPrice = order.ReserveBidPrice,
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
            BidderHoldPrice = command.ReserveBidPrice,
        };

        command.MatcherEvent = evt;
    }
}
