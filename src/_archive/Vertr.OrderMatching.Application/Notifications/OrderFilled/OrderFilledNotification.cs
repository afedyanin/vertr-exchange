using MediatR;

namespace Vertr.OrderMatching.Application.Notifications.OrderFullfilment
{
    public record class OrderFulfillmentNotification : INotification
    {
        public Guid OrderId { get; }

        public DateTime FulfillmentTime { get; }

        public decimal Price { get; }

        public decimal FulfillmentQty { get; }

        public decimal OrderQty { get; }

        public bool IsFullyFilled => FulfillmentQty == OrderQty;

        public bool IsPartiallyFilled => !IsFullyFilled;

        public OrderFulfillmentNotification(
            Guid orderId,
            DateTime fulfillmentTime,
            decimal price,
            decimal fullfilmentQty,
            decimal orderQty
            )
        {
            OrderId = orderId;
            FulfillmentTime = fulfillmentTime;
            Price = price;
            FulfillmentQty = fullfilmentQty;
            OrderQty = orderQty;
        }
    }
}
