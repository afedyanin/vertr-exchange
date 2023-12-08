using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Terminal.Server.Repositories;

public class OrderBookRepository : IOrderBookRepository
{
    private readonly Dictionary<int, OrderBook> _orderBooks = [];

    public Task<bool> Save(OrderBook orderBook)
    {
        if (!_orderBooks.TryAdd(orderBook.Symbol, orderBook))
        {
            _orderBooks[orderBook.Symbol] = orderBook;
        }

        return Task.FromResult(true);
    }

    public Task<OrderBook?> Get(int symbolId)
    {
        _orderBooks.TryGetValue(symbolId, out var orderBook);
        return Task.FromResult(orderBook);
    }

    public Task<OrderBook[]> GetList()
    {
        return Task.FromResult(_orderBooks.Values.ToArray());
    }
}
