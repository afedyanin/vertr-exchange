using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Client.SignalR.Requests;

public class HandleReduceRequest : IRequest
{
    public ReduceEvent? ReduceEvent { get; set; }
}
