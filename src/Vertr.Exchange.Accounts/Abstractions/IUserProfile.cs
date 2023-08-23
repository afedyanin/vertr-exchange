using Vertr.Exchange.Accounts.Enums;

namespace Vertr.Exchange.Accounts.Abstractions;

public interface IUserProfile
{
    long Uid { get; }

    UserStatus Status { get; }

    decimal AddToValue(int currency, decimal toBeAdded);

    bool Suspend();

    bool Resume();
}
