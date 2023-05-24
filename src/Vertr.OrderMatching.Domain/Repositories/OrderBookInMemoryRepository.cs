using Vertr.OrderMatching.Core.Books;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public class OrderBookInMemoryRepository : IOrderBookRepository
    {
        public OrderBook? Get(string instrument) => throw new NotImplementedException();
        public OrderBook GetOrAdd(string instrument) => throw new NotImplementedException();
        public bool Remove(string instrument) => throw new NotImplementedException();
    }
}

