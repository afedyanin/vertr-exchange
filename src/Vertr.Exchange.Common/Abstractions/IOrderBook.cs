namespace Vertr.Exchange.Common.Abstractions;

public interface IOrderBook
{
    IOrder? GetOrder(long orderId);

    bool RemoveOrder(IOrder order);

    bool AddNewOrder(IOrder order);

    bool UpdateOrder(IOrder order);

    L2MarketData GetL2MarketDataSnapshot(int size);

    long TryMatchInstantly(IOrder activeOrder, long filled, OrderCommand triggerCmd);
}
