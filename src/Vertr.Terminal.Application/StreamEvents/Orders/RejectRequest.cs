using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Application.StreamEvents.Orders;
public class RejectRequest : IRequest
{
    public RejectEvent? RejectEvent { get; set; }
}
