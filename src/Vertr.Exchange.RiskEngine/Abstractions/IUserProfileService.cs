using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Users;

namespace Vertr.Exchange.RiskEngine.Abstractions;

internal interface IUserProfileService
{
    UserProfile? GetUserProfile(long uid);

    UserProfile GetUserProfileOrAddSuspended(long uid);

    CommandResultCode BalanceAdjustment(long uid, int currency, decimal amount, long fundingTransactionId);

    bool AddEmptyUserProfile(long uid);

    CommandResultCode SuspendUserProfile(long uid);

    CommandResultCode ResumeUserProfile(long uid);

    void Reset();
}
