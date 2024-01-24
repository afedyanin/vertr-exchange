using Vertr.Terminal.Domain.MarketData;

namespace Vertr.Terminal.Domain.Abstractions;

public interface IMarketDataRepository
{
    Task<MarketDataItem> Update(int symbolId, DateTime timeStamp, decimal price);

    Task<MarketDataItem[]> GetSnapshot();

    Task<MarketDataItem[]> GetHistory(int symbolId);

    Task<MarketDataItem?> GetBySymbolId(int symbolId);

    Task Reset();
}
