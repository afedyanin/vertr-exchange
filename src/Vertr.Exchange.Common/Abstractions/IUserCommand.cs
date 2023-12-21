using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Common.Abstractions;
public interface IUserCommand
{
    CommandResultCode Execute();
}
