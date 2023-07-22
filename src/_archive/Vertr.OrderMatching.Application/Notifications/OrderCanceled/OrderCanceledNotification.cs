using MediatR;

namespace Vertr.OrderMatching.Application.Notifications.OrderCanceled
{
    public record class OrderCanceledNotification : INotification
    {
        public Guid OrderId { get; }

        public DateTime CancelTime { get; }

        public OrderCanceledNotification(
            Guid orderId,
            DateTime cancelTime)
        {
            OrderId = orderId;
            CancelTime = cancelTime;
        }
    }
}
