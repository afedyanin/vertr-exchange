using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Client.SignalR.Requests;

public class HandleOrderBookRequest : IRequest
{
    public OrderBook? OrderBook { get; set; }
}
