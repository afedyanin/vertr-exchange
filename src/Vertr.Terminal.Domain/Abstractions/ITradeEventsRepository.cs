using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Domain.Abstractions;

public interface ITradeEventsRepository
{
    Task<bool> Save(TradeEvent tradeEvent);

    Task<TradeEvent[]> GetList();

    Task Reset();
}
