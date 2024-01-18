using System.Collections.Concurrent;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Domain.MarketData;

namespace Vertr.Terminal.DataAccess.InMemory.Repositories;

internal class MarketDataRepository : IMarketDataRepository
{
    private readonly ConcurrentDictionary<int, MarketDataItem> _dict = new ConcurrentDictionary<int, MarketDataItem>();
    public Task<MarketDataItem?> GetBySymbolId(int symbolId)
    {
        _dict.TryGetValue(symbolId, out var res);
        return Task.FromResult(res);
    }

    public Task<MarketDataItem[]> GetSnapshot()
    {
        var res = _dict.Values.ToArray();
        return Task.FromResult(res);
    }

    public Task Reset()
    {
        _dict.Clear();
        return Task.CompletedTask;
    }

    public Task<MarketDataItem> Update(int symbolId, decimal price)
    {
        if (!_dict.TryGetValue(symbolId, out var item))
        {
            item = new MarketDataItem()
            {
                SymbolId = symbolId,
                Price = price
            };
        }

        var res = _dict.AddOrUpdate(
            symbolId,
            item,
            (symbolId, olditem) =>
            {
                olditem.Price = price;
                return olditem;
            });


        return Task.FromResult(res);
    }
}
