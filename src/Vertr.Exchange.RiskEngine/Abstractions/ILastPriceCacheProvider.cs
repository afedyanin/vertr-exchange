using Vertr.Exchange.RiskEngine.LastPriceCache;

namespace Vertr.Exchange.RiskEngine.Abstractions;
internal interface ILastPriceCacheProvider
{
    void AddLastPrice(int symbol, decimal askPrice, decimal bidPrice);

    LastPriceCacheRecord? GetLastPrice(int symbol);

    void Reset();
}
