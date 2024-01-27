using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.Common.Abstractions;

public interface IOrderBook
{
    IOrder? GetOrder(long orderId);

    IOrder[] GetOrdersByUid(long uid);

    bool RemoveOrder(IOrder order);

    bool AddOrder(IOrder order);

    long Reduce(IOrder order, long requestedReduceSize);

    L2MarketData GetL2MarketDataSnapshot(int size, long seq);

    MatcherResult TryMatchInstantly(OrderAction action, decimal price, long size, long filled = 0L);
}
