using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.MatchingEngine.OrderBooks;

namespace Vertr.Exchange.Domain.MatchingEngine.OrderBooks;
internal class OrderBookProvider(Func<IOrderBook> orderBookFactory) : IOrderBookProvider
{
    private readonly Func<IOrderBook> _orderBookFactory = orderBookFactory;

    // Key = Symbol
    private readonly Dictionary<int, IOrderBook> _orderBooks = [];

    public IOrderBook? GetOrderBook(int symbol)
    {
        if (_orderBooks.TryGetValue(symbol, out var orderBook))
        {
            return orderBook;
        }

        return null;
    }

    public void AddSymbol(int symbol)
    {
        if (!_orderBooks.ContainsKey(symbol))
        {
            _orderBooks.Add(symbol, _orderBookFactory());
        }
        else
        {
            // Warn: symbol already added.
        }
    }

    public void Reset()
    {
        _orderBooks.Clear();
    }

    public IDictionary<int, IOrder[]> GetOrders(long uid)
    {
        var res = new Dictionary<int, IOrder[]>();

        foreach (var key in _orderBooks.Keys)
        {
            var orders = _orderBooks[key].GetOrdersByUid(uid);
            res.Add(key, orders);
        }

        return res;
    }
}
