using MediatR;
using Microsoft.Extensions.Logging;

namespace Vertr.OrderMatching.Application.Commands.CancelOrder
{
    public sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly ILogger<CancelOrderCommandHandler> _logger;

        public CancelOrderCommandHandler(ILogger<CancelOrderCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cancel order command received: {buyLimit}", request);
            return Task.CompletedTask;
        }
    }
}
