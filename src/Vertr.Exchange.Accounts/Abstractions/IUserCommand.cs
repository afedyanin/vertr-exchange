using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts.Abstractions;
public interface IUserCommand
{
    CommandResultCode Execute();
}
