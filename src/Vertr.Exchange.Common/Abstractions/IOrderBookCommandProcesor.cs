using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;

public interface IOrderBookCommandProcesor
{
    CommandResultCode ProcessCommand(OrderCommand cmd);

    //L2MarketData GetL2MarketDataSnapshot(int size);
}
