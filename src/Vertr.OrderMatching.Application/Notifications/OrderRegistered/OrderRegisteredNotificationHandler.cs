using MediatR;
using Microsoft.Extensions.Logging;

namespace Vertr.OrderMatching.Application.Notifications.OrderRegistered
{
    public class OrderRegisteredNotificationHandler : INotificationHandler<OrderRegisteredNotification>
    {
        private readonly ILogger<OrderRegisteredNotificationHandler> _logger;

        public OrderRegisteredNotificationHandler(ILogger<OrderRegisteredNotificationHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderRegisteredNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order registered notification: {registered}", notification);
            return Task.CompletedTask;
        }
    }
}
