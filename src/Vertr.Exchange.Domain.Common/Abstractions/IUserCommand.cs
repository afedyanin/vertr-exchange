using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.Common.Abstractions;
public interface IUserCommand
{
    CommandResultCode Execute();
}
