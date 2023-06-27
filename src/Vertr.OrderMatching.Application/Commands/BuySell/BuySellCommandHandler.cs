using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Application.Notifications.OrderRegistered;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Commands.BuySell
{
    public class BuySellCommandHandler : IRequestHandler<BuySellCommand, BuySellCommandResult>
    {
        private readonly IOrderFactory _orderFactory;
        private readonly ILogger<BuySellCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IOrderRepository _repository;
        private readonly ITopicProvider<Order> _topicProvider;

        public BuySellCommandHandler(
            IMediator mediator,
            IOrderRepository repository,
            IOrderFactory orderFactory,
            ITopicProvider<Order> topicProvider,
            ILogger<BuySellCommandHandler> logger)
        {
            _mediator = mediator;
            _repository = repository;
            _topicProvider = topicProvider;
            _logger = logger;
            _orderFactory = orderFactory;
        }

        public async Task<BuySellCommandResult> Handle(BuySellCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buy/sell command received: {BuyLimit}", request);

            var order = _orderFactory.CreateOrder(
                request.CorrelationId,
                request.OwnerId,
                request.Ticker,
                request.Qty,
                request.Price,
                request.IsBuy);

            var validated = order.Validate();

            if (!validated.IsValid)
            {
                return new BuySellCommandResult(validated.ValidationErrors);
            }

            var orderTopic = _topicProvider.Get(order.Ticker);

            if (orderTopic == null)
            {
                return new BuySellCommandResult(new string[] { $"Ticker={order.Ticker} is not supported." });
            }

            var saved = _repository.Insert(order);

            if (!saved)
            {
                return new BuySellCommandResult(new string[] { $"Cannot register order CorrelcationId={order.CorrelationId}" });
            }

            await orderTopic.Produce(order, cancellationToken);

            // TODO: Отсылать регистрацию только если ордер попадет в книгу?
            var orderRegistered = new OrderRegisteredNotification(
                order.CorrelationId,
                order.Id,
                order.CreationTime);

            await _mediator.Publish(orderRegistered, cancellationToken);

            return new BuySellCommandResult(order.Id);
        }
    }
}
