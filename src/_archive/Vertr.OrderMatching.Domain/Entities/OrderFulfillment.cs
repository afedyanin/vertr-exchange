using Vertr.OrderMatching.Domain.Contracts;

namespace Vertr.OrderMatching.Domain.Entities
{
    public class OrderFulfillment : IEntity<Guid>
    {
        public Guid Id { get; }

        public Guid OrderId { get; }

        public Guid TradeId { get; }

        public decimal Price { get; }

        public decimal FilledQty { get; }

        public decimal RemainigQty { get; }

        public DateTime CreationTime { get; }

        private OrderFulfillment()
        {
        }

        internal OrderFulfillment(
            Guid id,
            Guid orderId,
            Guid tradeId,
            decimal price,
            decimal filledQty,
            decimal remainigQty,
            DateTime creationTime)
        {
            Id = id;
            OrderId = orderId;
            TradeId = tradeId;
            Price = price;
            FilledQty = filledQty;
            RemainigQty = remainigQty;
            CreationTime = creationTime;
        }
    }
}
