using MediatR;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public sealed class BuyCommandHandler : IRequestHandler<BuyLimitCommand>, IRequestHandler<BuyMarketCommand>
    {
        public Task Handle(BuyLimitCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(BuyMarketCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
