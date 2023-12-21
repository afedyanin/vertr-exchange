using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Application.StreamEvents.Orders;
public class TradeRequest : IRequest
{
    public TradeEvent? TradeEvent { get; set; }
}
