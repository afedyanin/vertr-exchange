using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Application.StreamEvents.Orders;

public class ReduceRequest : IRequest
{
    public ReduceEvent? ReduceEvent { get; set; }
}
