using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Common;
using Vertr.OrderMatching.Domain.Contracts;

namespace Vertr.OrderMatching.Application.Commands.Sell
{
    public sealed class SellCommandHandler :
        IRequestHandler<SellLimitCommand, BuySellCommandResult>,
        IRequestHandler<SellMarketCommand, BuySellCommandResult>
    {
        private readonly ILogger<SellCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IOrderFactory _orderFactory;

        public SellCommandHandler(
            ILogger<SellCommandHandler> logger,
            IMediator mediator,
            IOrderFactory orderFactory)
        {
            _logger = logger;
            _mediator = mediator;
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

            return await OrderHelper.HandleNewOrder(_mediator, order, cancellationToken);
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

            return await OrderHelper.HandleNewOrder(_mediator, order, cancellationToken);
        }
    }
}
