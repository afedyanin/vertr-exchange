using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Common;
using Vertr.OrderMatching.Domain.Contracts;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public sealed class BuyCommandHandler :
        IRequestHandler<BuyLimitCommand, BuySellCommandResult>,
        IRequestHandler<BuyMarketCommand, BuySellCommandResult>
    {
        private readonly ILogger<BuyCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IOrderFactory _orderFactory;

        public BuyCommandHandler(
            ILogger<BuyCommandHandler> logger,
            IMediator mediator,
            IOrderFactory orderFactory)
        {
            _logger = logger;
            _mediator = mediator;
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

            return await OrderHelper.HandleNewOrder(_mediator, order, cancellationToken);
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

            return await OrderHelper.HandleNewOrder(_mediator, order, cancellationToken);
        }
    }
}
