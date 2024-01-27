using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.SignalRClient.Requests;
public class HandleTradeRequest : IRequest
{
    public TradeEvent? TradeEvent { get; set; }
}
