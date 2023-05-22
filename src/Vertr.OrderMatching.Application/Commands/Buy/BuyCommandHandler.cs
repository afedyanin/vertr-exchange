using MediatR;
using Microsoft.Extensions.Logging;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public sealed class BuyCommandHandler : IRequestHandler<BuyLimitCommand>, IRequestHandler<BuyMarketCommand>
    {
        private readonly ILogger<BuyCommandHandler> _logger;

        public BuyCommandHandler(ILogger<BuyCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(BuyLimitCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buy limit command received: {buyLimit}", request);
            return Task.CompletedTask;
        }

        public Task Handle(BuyMarketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Buy market command received: {buyMarket}", request);
            return Task.CompletedTask;
        }
    }
}
