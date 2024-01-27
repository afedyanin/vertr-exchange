using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.SignalRClient.Requests;

public class HandleReduceRequest : IRequest
{
    public ReduceEvent? ReduceEvent { get; set; }
}
