using Vertr.OrderMatching.Core.Books;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public interface IOrderBookRepository
    {
        OrderBook? GetByTicker(string ticker);

        OrderBook GetOrAdd(string ticker);

        bool Remove(string ticker);
    }
}
