using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Server.Repositories;

internal sealed class TradeEventsRepository : ITradeEventsRepository
{
    private readonly Dictionary<long, TradeEvent> _tradeEvents = [];
    public Task<bool> Save(TradeEvent tradeEvent)
    {
        if (!_tradeEvents.TryAdd(tradeEvent.Seq, tradeEvent))
        {
            _tradeEvents[tradeEvent.Seq] = tradeEvent;
        }

        return Task.FromResult(true);
    }

    public Task<TradeEvent[]> GetList()
    {
        return Task.FromResult(_tradeEvents.Values.ToArray());
    }
}
