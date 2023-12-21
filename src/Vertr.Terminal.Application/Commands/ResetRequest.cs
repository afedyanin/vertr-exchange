using MediatR;
using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Application.Commands;

public class ResetRequest : IRequest<ApiCommandResult>
{
}
