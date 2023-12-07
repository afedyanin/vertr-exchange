using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Exchange.Server.Extensions;

internal static class OrderActionExtensions
{
    public static Common.Enums.OrderAction ToDomain(this OrderAction orderAction)
    {
        return orderAction switch
        {
            OrderAction.ASK => Common.Enums.OrderAction.ASK,
            OrderAction.BID => Common.Enums.OrderAction.BID,
            _ => throw new InvalidOperationException($"Unknown order action: {orderAction}"),
        };
    }
    public static OrderAction ToDto(this Common.Enums.OrderAction orderAction)
    {
        return orderAction switch
        {
            Common.Enums.OrderAction.ASK => OrderAction.ASK,
            Common.Enums.OrderAction.BID => OrderAction.BID,
            _ => throw new InvalidOperationException($"Unknown order action: {orderAction}"),
        };
    }
}
