using MediatR;

namespace Vertr.OrderMatching.Application.Commands.Sell
{
    public sealed class SellCommandHandler : IRequestHandler<SellLimitCommand>, IRequestHandler<SellMarketCommand>
    {
        public Task Handle(SellLimitCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(SellMarketCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
