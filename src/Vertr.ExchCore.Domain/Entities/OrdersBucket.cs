using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertr.ExchCore.Domain.Entities
{
    public class OrdersBucket
    {
        private readonly IDictionary<long, Order> _entries;

        public long Price { get; set; }

        public long TotalVolume { get; set; }

        public OrdersBucket(long price)
        {
            Price = price;
            TotalVolume = 0;
            _entries = new Dictionary<long, Order>();
        }

        public void Add(Order order)
        {
            _entries.Add(order.OrderId, order);
            TotalVolume += order.Size - order.Filled;
        }

        public Order Remove(long orderId, long uid)
        {
            _entries.TryGetValue(orderId, out Order order);
            if (order == null )

        }
    }
}
