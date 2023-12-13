using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Server.Repositories;

internal sealed class OrderBookSnapshotsRepository : IOrderBookSnapshotsRepository
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

    public Task Reset()
    {
        _orderBooks.Clear();
        return Task.CompletedTask;
    }
}
