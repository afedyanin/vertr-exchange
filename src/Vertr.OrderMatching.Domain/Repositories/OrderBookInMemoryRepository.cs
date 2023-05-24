using System.Collections.Concurrent;
using Vertr.OrderMatching.Core.Books;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public class OrderBookInMemoryRepository : IOrderBookRepository
    {
        private readonly ConcurrentDictionary<string, OrderBook> _books = new();

        public OrderBook? GetByTicker(string ticker)
        {
            var found = _books.TryGetValue(ticker, out var book);
            return found ? book : null;
        }

        public OrderBook GetOrAdd(string ticker)
        {
            return _books.GetOrAdd(ticker, new OrderBook());
        }

        public bool Remove(string ticker)
        {
            return _books.TryRemove(ticker, out var _);
        }
    }
}

