using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Grpc.Extensions;

internal static class EventTypeExtensions
{
    public static EventType ToGrpc(this EngineEventType evt)
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
