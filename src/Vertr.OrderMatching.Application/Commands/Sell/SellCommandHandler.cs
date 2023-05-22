using MediatR;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Commands.Buy;

namespace Vertr.OrderMatching.Application.Commands.Sell
{
    public sealed class SellCommandHandler : IRequestHandler<SellLimitCommand>, IRequestHandler<SellMarketCommand>
    {
        private readonly ILogger<SellCommandHandler> _logger;

        public SellCommandHandler(ILogger<SellCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(SellLimitCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sell limit command received: {sellLimit}", request);
            return Task.CompletedTask;
        }

        public Task Handle(SellMarketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sell market command received: {sellMarket}", request);
            return Task.CompletedTask;
        }
    }
}
