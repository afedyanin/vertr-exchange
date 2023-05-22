using MediatR;

namespace Vertr.OrderMatching.Application.Commands.CancelOrder
{
    public sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        public Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
