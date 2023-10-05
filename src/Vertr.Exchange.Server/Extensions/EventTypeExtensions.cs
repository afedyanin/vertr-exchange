using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class EventTypeExtensions
{
    public static EventType ToProto(this EngineEventType evt)
    {
        return evt switch
        {
            EngineEventType.BINARY_EVENT => EventType.BinaryEvent,
            EngineEventType.TRADE => EventType.Trade,
            EngineEventType.REJECT => EventType.Reject,
            EngineEventType.REDUCE => EventType.Reduce,
            _ => EventType.Trade,
        };
    }
}
