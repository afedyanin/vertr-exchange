using MediatR;
using Vertr.OrderMatching.Application.Notifications.OrderRegistered;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Common
{
    public abstract class BuySellCommandHandlerBase
    {
        protected IMediator Mediator { get; }

        protected IOrderRepository Repository { get; }

        protected BuySellCommandHandlerBase(
            IMediator mediator,
            IOrderRepository repository)
        {
            Mediator = mediator;
            Repository = repository;
        }

        protected async Task<BuySellCommandResult> HandleNewOrder(Order order, CancellationToken cancellationToken)
        {
            var validated = order.Validate();

            if (!validated.IsValid)
            {
                return new BuySellCommandResult(validated.ValidationErrors);
            }

            var saved = Repository.Insert(order);

            if (!saved)
            {
                return new BuySellCommandResult(new string[] {$"Cannot register order CorrelcationId={order.CorrelationId}"});
            }

            var orderRegistered = new OrderRegisteredNotification(
                order.CorrelationId,
                order.Id,
                order.CreationTime);

            await Mediator.Publish(orderRegistered, cancellationToken);

            // TODO: Produce order to topic

            return new BuySellCommandResult(order.Id);
        }
    }
}
