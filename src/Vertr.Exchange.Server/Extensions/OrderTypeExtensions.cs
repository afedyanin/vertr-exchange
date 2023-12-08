using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Exchange.Server.Extensions;

internal static class OrderTypeExtensions
{
    public static Common.Enums.OrderType ToDomain(this OrderType orderType)
    {
        return orderType switch
        {
            OrderType.IOC => Common.Enums.OrderType.IOC,
            OrderType.GTC => Common.Enums.OrderType.GTC,
            OrderType.IOC_BUDGET => Common.Enums.OrderType.IOC_BUDGET,
            OrderType.FOK => Common.Enums.OrderType.FOK,
            OrderType.FOK_BUDGET => Common.Enums.OrderType.FOK_BUDGET,
            _ => throw new InvalidOperationException($"Unknown order action: {orderType}"),
        };
    }
}
