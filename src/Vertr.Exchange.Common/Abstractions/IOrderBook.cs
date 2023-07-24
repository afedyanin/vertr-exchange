namespace Vertr.Exchange.Common.Abstractions;

public interface IOrderBook
{
    CommandResultCode ProcessCommand(OrderCommand cmd);

    L2MarketData GetL2MarketDataSnapshot(int size);
}
