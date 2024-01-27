using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.SignalRClient.Requests;
public class RejectRequest : IRequest
{
    public RejectEvent? RejectEvent { get; set; }
}
