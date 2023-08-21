using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.LastPriceCache;

internal class LastPriceCacheProvider : ILastPriceCacheProvider
{
    // symbol
    private readonly IDictionary<int, LastPriceCacheRecord> _lastPriceCache;

    public LastPriceCacheProvider()
    {
        _lastPriceCache = new Dictionary<int, LastPriceCacheRecord>();
    }

    public void AddLastPrice(int symbol, decimal askPrice, decimal bidPrice)
    {
        if (_lastPriceCache.ContainsKey(symbol))
        {
            _lastPriceCache[symbol] = new LastPriceCacheRecord(askPrice, bidPrice);
        }
        else
        {
            _lastPriceCache.Add(symbol, new LastPriceCacheRecord(askPrice, bidPrice));
        }
    }

    public LastPriceCacheRecord? GetLastPrice(int symbol)
    {
        _lastPriceCache.TryGetValue(symbol, out var priceCache);
        return priceCache;
    }

    public void Reset()
    {
        _lastPriceCache.Clear();
    }
}
