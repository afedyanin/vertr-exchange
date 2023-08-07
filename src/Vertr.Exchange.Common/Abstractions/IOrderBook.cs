namespace Vertr.Exchange.Common.Abstractions;

public interface IOrderBook
{
    IOrder? GetOrder(long orderId);

    bool RemoveOrder(IOrder order);

    bool AddOrder(IOrder order);

    long Reduce(IOrder order, long requestedReduceSize);

    L2MarketData GetL2MarketDataSnapshot(int size);

    long TryMatchInstantly(OrderCommand orderCommand, long filled = 0L);
}
