using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Api.Extensions;

public static class OrderActionExtensions
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
}
