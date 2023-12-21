using MediatR;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Terminal.Application.Commands.Orders;

public class MoveRequest : IRequest<ApiCommandResult>
{
    public MoveOrderRequest? MoveOrderRequest { get; init; }
}
