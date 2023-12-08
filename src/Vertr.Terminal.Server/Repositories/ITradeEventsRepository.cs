using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.Server.Repositories;

public interface ITradeEventsRepository
{
    Task<bool> Save(TradeItem[] tradeItems);

    Task<TradeItem[]> GetList();
}
