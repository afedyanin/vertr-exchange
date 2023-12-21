using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Application.StreamEvents;

public class OrderBookRequest : IRequest
{
    public OrderBook? OrderBook { get; set; }
}
