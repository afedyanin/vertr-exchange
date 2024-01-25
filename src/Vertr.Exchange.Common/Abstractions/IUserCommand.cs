using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.Common.Abstractions;
public interface IUserCommand
{
    CommandResultCode Execute();
}
