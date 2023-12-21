using MediatR;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Terminal.Application.Commands.Orders;
public class CancelRequest : IRequest<ApiCommandResult>
{
    public CancelOrderRequest? CancelOrderRequest { get; init; }
}
