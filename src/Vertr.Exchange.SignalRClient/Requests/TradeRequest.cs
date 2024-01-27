using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.SignalRClient.Requests;
public class TradeRequest : IRequest
{
    public TradeEvent? TradeEvent { get; set; }
}
