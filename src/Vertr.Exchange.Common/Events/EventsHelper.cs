using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Events;

public static class EventsHelper
{
    public static void AttachMatcherEvents(
        OrderCommand command,
        IEnumerable<IEngineEvent> tradeEvents)
    {
        IEngineEvent? eventsTail = null;

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
            BinaryData = data ?? Array.Empty<byte>(),
        };

        command.EngineEvent = evt;
    }

    public static IEngineEvent? GetFirstBinaryEvent(OrderCommand command)
    {
        var res = command.EngineEvent;

        if (res == null)
        {
            return null;
        }

        while (res != null && res.EventType != EngineEventType.BINARY_EVENT)
        {
            res = res.NextEvent;
        }

        return res;
    }
}
