using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Server.Repositories;

public interface IOrderBookSnapshotsRepository
{
    Task<bool> Save(OrderBook orderBook);

    Task<OrderBook?> Get(int symbolId);

    Task<OrderBook[]> GetList();
}