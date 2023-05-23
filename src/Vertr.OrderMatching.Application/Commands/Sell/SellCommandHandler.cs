using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Common;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Commands.Sell
{
    public sealed class SellCommandHandler : BuySellCommandHandlerBase,
        IRequestHandler<SellLimitCommand, BuySellCommandResult>,
        IRequestHandler<SellMarketCommand, BuySellCommandResult>
    {
        private readonly ILogger<SellCommandHandler> _logger;
        private readonly IOrderFactory _orderFactory;

        public SellCommandHandler(
            ILogger<SellCommandHandler> logger,
            IMediator mediator,
            IOrderFactory orderFactory,
            IOrderRepository repository) : base(mediator, repository)
        {
            _logger = logger;
            _orderFactory = orderFactory;
        }

        public async Task<BuySellCommandResult> Handle(SellLimitCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sell limit command received: {sellLimit}", request);

            var order = _orderFactory.CreateOrder(
                request.CorrelationId,
                request.OwnerId,
                request.Instrument,
                request.Qty,
                request.Price,
                false);

            return await HandleNewOrder(order, cancellationToken);
        }

        public async Task<BuySellCommandResult> Handle(SellMarketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sell market command received: {sellMarket}", request);

            var order = _orderFactory.CreateOrder(
                request.CorrelationId,
                request.OwnerId,
                request.Instrument,
                request.Qty,
                decimal.Zero,
                false);

            return await HandleNewOrder(order, cancellationToken);
        }
    }
}
