using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Adjustments;
using Vertr.Exchange.RiskEngine.Users;

namespace Vertr.Exchange.RiskEngine.Abstractions;

internal interface IUserProfileService
{
    UserProfile? GetUserProfile(long uid);

    UserProfile GetUserProfileOrAddSuspended(long uid);

    CommandResultCode BalanceAdjustment(
        long uid,
        int currency,
        decimal amount,
        long fundingTransactionId,
        BalanceAdjustmentType balanceAdjustmentType);

    bool AddEmptyUserProfile(long uid);

    void BatchAddAccounts(IDictionary<int, IDictionary<int, long>> users);

    CommandResultCode SuspendUserProfile(long uid);

    CommandResultCode ResumeUserProfile(long uid);

    void Reset();
}
