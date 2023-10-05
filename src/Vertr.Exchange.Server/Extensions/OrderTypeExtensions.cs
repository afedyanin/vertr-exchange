using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class OrderTypeExtensions
{
    public static Common.Enums.OrderType ToDomain(this OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Ioc => Common.Enums.OrderType.IOC,
            OrderType.Gtc => Common.Enums.OrderType.GTC,
            OrderType.IocBudget => Common.Enums.OrderType.IOC_BUDGET,
            OrderType.Fok => Common.Enums.OrderType.FOK,
            OrderType.FokBudget => Common.Enums.OrderType.FOK_BUDGET,
            _ => Common.Enums.OrderType.GTC,
        };
    }
}
