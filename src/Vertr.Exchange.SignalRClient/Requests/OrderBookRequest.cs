using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.SignalRClient.Requests;

public class OrderBookRequest : IRequest
{
    public OrderBook? OrderBook { get; set; }
}
