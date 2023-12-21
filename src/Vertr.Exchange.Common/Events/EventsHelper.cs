using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Common.Events;

public static class EventsHelper
{
    public static void AttachMatcherEvents(
        OrderCommand command,
        IEnumerable<IEngineEvent> tradeEvents)
    {
        IEngineEvent? eventsTail = command.EngineEvent;

        foreach (var evt in tradeEvents)
        {
            if (eventsTail == null)
            {
                command.EngineEvent = evt;
            }
            else
            {
                eventsTail.NextEvent = evt;
            }

            eventsTail = evt;
        }
    }

    public static void AttachReduceEvent(
        OrderCommand command,
        IOrder order,
        long reduceSize,
        bool completed)
    {
        var evt = new EngineEvent
        {
            EventType = EngineEventType.REDUCE,
            ActiveOrderCompleted = completed,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = order.Price,
            Size = reduceSize,
        };

        command.EngineEvent = evt;
    }

    public static void AttachRejectEvent(
        OrderCommand command,
        decimal price,
        long rejectedSize)
    {
        var evt = new EngineEvent
        {
            EventType = EngineEventType.REJECT,
            ActiveOrderCompleted = true,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = price,
            Size = rejectedSize,
        };

        command.EngineEvent = evt;
    }

    public static void AttachBinaryEvent(
        OrderCommand command,
        byte[] data)
    {

        var evt = new EngineEvent
        {
            EventType = EngineEventType.BINARY_EVENT,
            BinaryData = data ?? [],
        };

        command.EngineEvent = evt;
    }
}
