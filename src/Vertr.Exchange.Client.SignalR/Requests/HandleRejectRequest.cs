using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Client.SignalR.Requests;
public class HandleRejectRequest : IRequest
{
    public RejectEvent? RejectEvent { get; set; }
}
