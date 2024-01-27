using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.SignalRClient.Requests;

public class ReduceRequest : IRequest
{
    public ReduceEvent? ReduceEvent { get; set; }
}
