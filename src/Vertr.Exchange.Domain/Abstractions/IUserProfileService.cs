using Vertr.Exchange.Domain.Enums;
using Vertr.Exchange.Domain.Users;

namespace Vertr.Exchange.Domain.Abstractions;

internal interface IUserProfileService
{
    UserProfile? GetUserProfile(long uid);

    UserProfile GetUserProfileOrAddSuspended(long uid);

    CommandResultCode BalanceAdjustment(long uid, int symbol, long amount, long fundingTransactionId);

    bool AddEmptyUserProfile(long uid);

    CommandResultCode SuspendUserProfile(long uid);

    CommandResultCode ResumeUserProfile(long uid);

    void Reset();
}
