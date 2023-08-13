using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;

public interface IOrderBook
{
    IOrder? GetOrder(long orderId);

    bool RemoveOrder(IOrder order);

    bool AddOrder(IOrder order);

    long Reduce(IOrder order, long requestedReduceSize);

    L2MarketData GetL2MarketDataSnapshot(int size);

    MatcherResult TryMatchInstantly(OrderAction action, decimal price, long size, long filled = 0L);
}
