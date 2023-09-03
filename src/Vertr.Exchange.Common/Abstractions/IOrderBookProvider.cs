using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.MatchingEngine.OrderBooks;
public interface IOrderBookProvider
{
    IOrderBook? GetOrderBook(int symbol);

    IDictionary<int, IOrder[]> GetOrders(long uid);

    void AddSymbol(int symbol);

    void Reset();
}
