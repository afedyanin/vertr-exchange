using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;
internal static class OrderStub
{
    public static IOrder CreateBidOrder(decimal price, long size, long filled = 0L)
        => CreateOrder(OrderAction.BID, price, size, filled);

    public static IOrder CreateAskOrder(decimal price, long size, long filled = 0L)
        => CreateOrder(OrderAction.ASK, price, size, filled);

    public static IOrder CreateOrder(OrderAction action, decimal price, long size, long filled = 0L)
    {
        var order = new Order(
            action,
            orderId: OrderGen.NextId,
            price: price,
            reserveBidPrice: decimal.Zero,
            size: size,
            filled: filled,
            uid: OrderGen.UserId,
            timestamp: OrderGen.NextTime);

        return order;
    }
}
