using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Abstractions;

public interface IOrderBook
{
    CommandResultCode ProcessCommand(OrderCommand orderCommand);

    L2MarketData GetL2MarketDataSnapshot(int size);
}
