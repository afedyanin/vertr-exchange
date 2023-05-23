using Vertr.OrderMatching.Core.Books;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public interface IOrderBookRepository
    {
        OrderBook? Get(string instrument);

        OrderBook GetOrAdd(string instrument);

        bool Remove(string instrument);
    }
}
