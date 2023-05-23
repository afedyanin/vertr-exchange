using MediatR;
using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Application.Notifications.OrderRegistered;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Common
{
    public abstract class BuySellCommandHandlerBase
    {
        protected IMediator Mediator { get; }

        protected IOrderRepository Repository { get; }

        protected ITopicProvider<Order> TopicProvider { get; }

        protected BuySellCommandHandlerBase(
            IMediator mediator,
            IOrderRepository repository,
            ITopicProvider<Order> topicProvider)
        {
            Mediator = mediator;
            Repository = repository;
            TopicProvider = topicProvider;
        }

        protected async Task<BuySellCommandResult> HandleNewOrder(Order order, CancellationToken cancellationToken)
        {
            var validated = order.Validate();

            if (!validated.IsValid)
            {
                return new BuySellCommandResult(validated.ValidationErrors);
            }

            var topic = TopicProvider.Get(order.Instrument);

            if (topic == null)
            {
                return new BuySellCommandResult(new string[] { $"Instrument={order.Instrument} is not supported." });
            }

            var saved = Repository.Insert(order);

            if (!saved)
            {
                return new BuySellCommandResult(new string[] {$"Cannot register order CorrelcationId={order.CorrelationId}"});
            }

            await topic.Produce(order, cancellationToken);

            var orderRegistered = new OrderRegisteredNotification(
                order.CorrelationId,
                order.Id,
                order.CreationTime);

            await Mediator.Publish(orderRegistered, cancellationToken);

            return new BuySellCommandResult(order.Id);
        }
    }
}
