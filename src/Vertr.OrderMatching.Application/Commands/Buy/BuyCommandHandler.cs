using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Notifications.OrderRegistered;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public sealed class BuyCommandHandler : IRequestHandler<BuyLimitCommand>, IRequestHandler<BuyMarketCommand>
    {
        private readonly ILogger<BuyCommandHandler> _logger;
        private readonly IMediator _mediator;

        public BuyCommandHandler(
            ILogger<BuyCommandHandler> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Handle(BuyLimitCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buy limit command received: {buyLimit}", request);

            var orderRegistered = new OrderRegisteredNotification(
                request.CorrelationId,
                Guid.NewGuid(),
                DateTime.UtcNow);

            await _mediator.Publish(orderRegistered, cancellationToken);
        }

        public async Task Handle(BuyMarketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buy market command received: {buyMarket}", request);

            var orderRegistered = new OrderRegisteredNotification(
                request.CorrelationId,
                Guid.NewGuid(),
                DateTime.UtcNow);

            await _mediator.Publish(orderRegistered, cancellationToken);
        }
    }
}
