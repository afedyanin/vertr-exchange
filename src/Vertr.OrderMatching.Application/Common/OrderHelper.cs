using MediatR;
using Vertr.OrderMatching.Application.Notifications.OrderRegistered;
using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Application.Common
{
    internal static class OrderHelper
    {
        public static async Task<BuySellCommandResult> HandleNewOrder(
            IMediator mediator,
            Order order,
            CancellationToken cancellationToken)
        {
            var validated = order.Validate();

            if (!validated.IsValid)
            {
                return new BuySellCommandResult(validated.ValidationErrors);
            }

            // TODO: Save order

            var orderRegistered = new OrderRegisteredNotification(
                order.CorrelationId,
                order.Id,
                order.CreationTime);

            await mediator.Publish(orderRegistered, cancellationToken);

            // TODO: Produce order to topic

            return new BuySellCommandResult(order.Id);
        }
    }
}
