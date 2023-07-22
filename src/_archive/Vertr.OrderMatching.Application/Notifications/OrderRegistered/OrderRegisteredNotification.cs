using MediatR;

namespace Vertr.OrderMatching.Application.Notifications.OrderRegistered
{
    public record class OrderRegisteredNotification : INotification
    {
        public Guid CorrelationId { get; }

        public Guid OrderId { get; }

        public DateTime RegisteredTime { get; }

        public OrderRegisteredNotification(
            Guid correlationId,
            Guid orderId,
            DateTime registeredTime)
        {
            CorrelationId = correlationId;
            OrderId = orderId;
            RegisteredTime = registeredTime;
        }
    }
}
