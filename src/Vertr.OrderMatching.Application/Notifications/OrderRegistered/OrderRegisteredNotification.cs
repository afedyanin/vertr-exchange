using MediatR;

namespace Vertr.OrderMatching.Application.Notifications.OrderRegistered
{
    public record class OrderRegisteredNotification : INotification
    {
        public Guid CorrelationId { get; }

        public Guid OrderId { get; }

        public DateTime RegesteredTime { get; }
    }
}
