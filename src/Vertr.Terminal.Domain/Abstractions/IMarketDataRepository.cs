using Vertr.Terminal.Domain.MarketData;

namespace Vertr.Terminal.Domain.Abstractions;

public interface IMarketDataRepository
{
    Task<MarketDataItem> Update(int symbolId, decimal price);

    Task<MarketDataItem[]> GetSnapshot();

    Task<MarketDataItem?> GetBySymbolId(int symbolId);

    Task Reset();
}
