using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.MatchingEngine.OrderBooks;
internal class OrderBookProvider : IOrderBookProvider
{
    private readonly Func<IOrderBook> _orderBookFactory;

    // Key = Symbol
    private readonly IDictionary<int, IOrderBook> _orderBooks;

    public OrderBookProvider(Func<IOrderBook> orderBookFactory)
    {
        _orderBooks = new Dictionary<int, IOrderBook>();
        _orderBookFactory = orderBookFactory;
    }

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
