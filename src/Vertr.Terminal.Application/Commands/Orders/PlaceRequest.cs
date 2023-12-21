using MediatR;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Terminal.Application.Commands.Orders;
public class PlaceRequest : IRequest<ApiCommandResult>
{
    public PlaceOrderRequest? PlaceOrderRequest { get; init; }
}
