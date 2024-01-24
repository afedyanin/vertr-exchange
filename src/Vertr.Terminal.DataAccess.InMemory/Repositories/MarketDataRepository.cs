using System.Collections.Concurrent;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Domain.MarketData;

namespace Vertr.Terminal.DataAccess.InMemory.Repositories;

internal class MarketDataRepository : IMarketDataRepository
{
    private readonly ConcurrentDictionary<int, MarketDataItem> _dict = new ConcurrentDictionary<int, MarketDataItem>();
    private readonly ConcurrentDictionary<int, IList<MarketDataItem>> _history = new ConcurrentDictionary<int, IList<MarketDataItem>>();

    public Task<MarketDataItem?> GetBySymbolId(int symbolId)
    {
        _dict.TryGetValue(symbolId, out var res);
        return Task.FromResult(res);
    }

    public Task<MarketDataItem[]> GetHistory(int symbolId)
    {
        if (!_history.TryGetValue(symbolId, out var items))
        {
            return Task.FromResult(Array.Empty<MarketDataItem>());
        }

        var res = items.ToArray();
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
        _history.Clear();
        return Task.CompletedTask;
    }

    public Task<MarketDataItem> Update(int symbolId, DateTime timeStamp, decimal price)
    {
        if (!_dict.TryGetValue(symbolId, out var item))
        {
            item = new MarketDataItem()
            {
                SymbolId = symbolId,
                TimeStamp = timeStamp,
                Price = price,
                DayHigh = price,
                DayLow = price,
                DayOpen = price,
                LastChange = price,
            };
        }

        var updated = item.Update(timeStamp, price);

        var res = _dict.AddOrUpdate(
            symbolId,
            updated,
            (symbolId, olditem) =>
            {
                return updated;
            });

        AddToHistory(res);

        return Task.FromResult(res);
    }

    private void AddToHistory(MarketDataItem item)
    {
        if (!_history.TryGetValue(item.SymbolId, out var itemList))
        {
            itemList = new List<MarketDataItem>();
        }

        var list = _history.AddOrUpdate(
            item.SymbolId,
            itemList,
            (symbolId, oldList) =>
            {
                return oldList;
            });

        list.Add(item);
    }
}
