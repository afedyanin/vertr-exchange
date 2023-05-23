using System.Collections.Concurrent;
using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public class OrderInMemoryRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<Guid, Order> _dictionary = new ConcurrentDictionary<Guid, Order>();


        public bool Delete(Guid orderId)
        {
            return _dictionary.TryRemove(orderId, out _);
        }

        public Order[] GetAll()
        {
            return _dictionary.Values.ToArray();
        }

        public Order? GetById(Guid orderId)
        {
            var res = _dictionary.TryGetValue(orderId, out var order);
            return res ? order : null;
        }

        public bool Insert(Order order)
        {
            return _dictionary.TryAdd(order.Id, order);
        }
    }
}
