using MediatR;

namespace Vertr.OrderMatching.Application.Commands.CancelOrder
{
    public record class CancelOrderCommand : IRequest
    {
        public Guid OrderId { get; }

        public CancelOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
