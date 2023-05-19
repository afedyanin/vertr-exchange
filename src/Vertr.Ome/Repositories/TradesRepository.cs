using System.Collections.Concurrent;
using Vertr.Ome.Contracts;
using Vertr.Ome.Entities;

namespace Vertr.Ome.Repositories
{
    public class TradesRepository : ITradesRepository
    {
        private readonly ConcurrentDictionary<long, Trade> _trades;

        public TradesRepository()
        {
            _trades = new ConcurrentDictionary<long, Trade>();
        }

        public bool Add(Trade trade)
        {
            var added = _trades.TryAdd(trade.Id, trade);

            return added;
        }

        public bool Remove(long id)
        {
            var removed = _trades.TryRemove(id, out var _);

            return removed;
        }

        public ICollection<Trade> GetAll()
        {
            var trades = _trades.Values;

            return trades;
        }

        public Trade GetById(long id)
        {
            var found = _trades.TryGetValue(id, out var trade);

            return found ? trade! : Trade.Empty;
        }
    }
}
