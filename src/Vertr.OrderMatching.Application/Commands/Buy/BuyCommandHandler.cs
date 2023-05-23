using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Common;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public sealed class BuyCommandHandler : BuySellCommandHandlerBase,
        IRequestHandler<BuyLimitCommand, BuySellCommandResult>,
        IRequestHandler<BuyMarketCommand, BuySellCommandResult>
    {
        private readonly IOrderFactory _orderFactory;
        private readonly ILogger<BuyCommandHandler> _logger;

        public BuyCommandHandler(
            IMediator mediator,
            IOrderRepository repository,
            IOrderFactory orderFactory,
            ILogger<BuyCommandHandler> logger) : base(mediator, repository)
        {
            _logger = logger;
            _orderFactory = orderFactory;
        }

        public async Task<BuySellCommandResult> Handle(BuyLimitCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buy limit command received: {buyLimit}", request);

            var order = _orderFactory.CreateOrder(
                request.CorrelationId,
                request.OwnerId,
                request.Instrument,
                request.Qty,
                decimal.Zero,
                true);

            return await HandleNewOrder(order, cancellationToken);
        }

        public async Task<BuySellCommandResult> Handle(BuyMarketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buy market command received: {buyMarket}", request);

            var order = _orderFactory.CreateOrder(
                request.CorrelationId,
                request.OwnerId,
                request.Instrument,
                request.Qty,
                decimal.Zero,
                true);

            return await HandleNewOrder(order, cancellationToken);
        }
    }
}
