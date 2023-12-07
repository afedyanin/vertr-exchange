using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Server.Extensions;

internal static class EventTypeExtensions
{
    public static Contracts.Enums.EngineEventType ToDto(this EngineEventType evt)
    {
        return evt switch
        {
            EngineEventType.BINARY_EVENT => Contracts.Enums.EngineEventType.BINARY_EVENT,
            EngineEventType.TRADE => Contracts.Enums.EngineEventType.TRADE,
            EngineEventType.REJECT => Contracts.Enums.EngineEventType.REJECT,
            EngineEventType.REDUCE => Contracts.Enums.EngineEventType.REDUCE,
            _ => throw new InvalidOperationException($"Unknown EngineEventType={evt}"),
        };
    }
}
