using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Terminal.Server.Repositories;

public interface IOrderBookRepository
{
    Task<bool> Save(OrderBook orderBook);

    Task<OrderBook?> Get(int symbolId);

    Task<OrderBook[]> GetList();
}
