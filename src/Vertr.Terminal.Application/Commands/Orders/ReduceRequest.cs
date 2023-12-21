using MediatR;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Terminal.Application.Commands.Orders;

public class ReduceRequest : IRequest<ApiCommandResult>
{
    public ReduceOrderRequest? ReduceOrderRequest { get; init; }
}
