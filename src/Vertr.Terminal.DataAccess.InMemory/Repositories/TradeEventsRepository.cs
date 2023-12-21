using Vertr.Exchange.Contracts;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.DataAccess.InMemory.Repositories;

internal sealed class TradeEventsRepository : ITradeEventsRepository
{
    private readonly Dictionary<long, TradeEvent> _tradeEvents = [];
    public Task<bool> Save(TradeEvent tradeEvent)
    {
        _tradeEvents.Add(tradeEvent.Seq, tradeEvent);
        return Task.FromResult(true);
    }

    public Task<TradeEvent[]> GetList()
    {
        var res = _tradeEvents.Values.OrderBy(ti => ti.Seq).ToArray();
        return Task.FromResult(res);
    }

    public Task Reset()
    {
        _tradeEvents.Clear();
        return Task.CompletedTask;
    }
}
