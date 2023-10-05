using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class OrderActionExtensions
{
    public static Common.Enums.OrderAction ToDomain(this OrderAction orderAction)
    {
        return orderAction switch
        {
            OrderAction.Ask => Common.Enums.OrderAction.ASK,
            OrderAction.Bid => Common.Enums.OrderAction.BID,
            _ => Common.Enums.OrderAction.ASK,
        };
    }
    public static OrderAction ToProto(this Common.Enums.OrderAction orderAction)
    {
        return orderAction switch
        {
            Common.Enums.OrderAction.ASK => OrderAction.Ask,
            Common.Enums.OrderAction.BID => OrderAction.Bid,
            _ => OrderAction.Ask,
        };
    }
}
