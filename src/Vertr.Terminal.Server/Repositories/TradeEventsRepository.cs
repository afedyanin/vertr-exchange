using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.Server.Repositories;

internal sealed class TradeEventsRepository : ITradeEventsRepository
{
    private readonly List<TradeItem> _tradeItems = [];
    public Task<bool> Save(TradeItem[] tradeItems)
    {
        _tradeItems.AddRange(tradeItems);
        return Task.FromResult(true);
    }

    public Task<TradeItem[]> GetList()
    {
        var res = _tradeItems.OrderBy(ti => ti.Seq).ToArray();
        return Task.FromResult(res);
    }
}
