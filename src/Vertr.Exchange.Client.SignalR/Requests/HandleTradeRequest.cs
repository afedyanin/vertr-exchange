using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Client.SignalR.Requests;
public class HandleTradeRequest : IRequest
{
    public TradeEvent? TradeEvent { get; set; }
}
