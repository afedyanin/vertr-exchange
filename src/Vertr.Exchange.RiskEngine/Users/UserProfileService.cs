using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.RiskEngine.Adjustments;

namespace Vertr.Exchange.RiskEngine.Users;

internal sealed class UserProfileService : IUserProfileService
{
    private readonly IDictionary<long, UserProfile> _userProfiles;
    private readonly IAdjustmentsService _adjustmentsService;

    public UserProfileService(IAdjustmentsService adjustmentsService)
    {
        _userProfiles = new Dictionary<long, UserProfile>();
        _adjustmentsService = adjustmentsService;
    }

    public UserProfile? GetUserProfile(long uid)
    {
        return _userProfiles.TryGetValue(uid, out var userProfile) ? userProfile : null;
    }

    public UserProfile GetUserProfileOrAddSuspended(long uid)
    {
        _userProfiles.TryGetValue(uid, out var profile);

        if (profile != null)
        {
            return profile;
        }

        profile = new UserProfile(uid, UserStatus.SUSPENDED);
        _userProfiles.Add(uid, profile);

        return profile;
    }

    public bool AddEmptyUserProfile(long uid)
    {
        _userProfiles.TryGetValue(uid, out var profile);

        if (profile == null)
        {
            profile = new UserProfile(uid, UserStatus.ACTIVE);
            _userProfiles.Add(uid, profile);
            return true;
        }

        // log: user already exists
        return false;
    }

    public CommandResultCode SuspendUserProfile(long uid)
    {
        _userProfiles.TryGetValue(uid, out var profile);

        if (profile == null)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_FOUND;

        }
        else if (profile.UserStatus == UserStatus.SUSPENDED)
        {
            return CommandResultCode.USER_MGMT_USER_ALREADY_SUSPENDED;

        }
        else if (profile.HasPositions)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS;

        }
        else if (profile.HasAccounts)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS;
        }
        else
        {
            _userProfiles.Remove(uid);
            return CommandResultCode.SUCCESS;
        }
    }

    public CommandResultCode ResumeUserProfile(long uid)
    {
        _userProfiles.TryGetValue(uid, out var profile);

        if (profile == null)
        {
            // create new empty user profile
            // account balance adjustments should be applied later
            _userProfiles.Add(uid, new UserProfile(uid, UserStatus.ACTIVE));
            return CommandResultCode.SUCCESS;
        }
        else if (profile.UserStatus != UserStatus.SUSPENDED)
        {
            // attempt to resume non-suspended account (or resume twice)
            return CommandResultCode.USER_MGMT_USER_NOT_SUSPENDED;
        }
        else
        {
            // resume existing suspended profile (can contain non empty positions or accounts)
            profile.UserStatus = UserStatus.ACTIVE;
            //log.debug("Resumed user profile: {}", userProfile);
            return CommandResultCode.SUCCESS;
        }
    }

    public void Reset()
    {
        _userProfiles.Clear();
        _adjustmentsService.Reset();
    }

    public CommandResultCode BalanceAdjustment(
        long uid,
        int currency,
        decimal amount,
        long fundingTransactionId,
        BalanceAdjustmentType balanceAdjustmentType)
    {
        _userProfiles.TryGetValue(uid, out var profile);

        if (profile == null)
        {
            // log.warn("User profile {} not found", uid);
            return CommandResultCode.AUTH_INVALID_USER;
        }

        // double settlement protection
        if (profile.AdjustmentsCounter == fundingTransactionId)
        {
            return CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_SAME;
        }
        if (profile.AdjustmentsCounter > fundingTransactionId)
        {
            return CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_ALREADY_APPLIED_MANY;
        }

        // validate balance for withdrawals
        var currentAmount = profile.GetCurrentAmount(currency);

        if (amount < 0 && (currentAmount + amount) < 0)
        {
            return CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_NSF;
        }

        profile.AdjustmentsCounter = fundingTransactionId;
        profile.AddToValue(currency, amount);

        _adjustmentsService.AddAdjustment(
            currency,
            amount,
            balanceAdjustmentType);

        //log.debug("FUND: {}", userProfile);
        return CommandResultCode.SUCCESS;
    }

    public void BatchAddAccounts(IDictionary<int, IDictionary<int, long>> users)
    {
        foreach (var (uid, acounts) in users)
        {
            if (!AddEmptyUserProfile(uid))
            {
                // log.debug("User already exist: {}", uid);
                continue;
            }
            foreach (var (currency, balance) in acounts)
            {
                BalanceAdjustment(
                    uid,
                    currency,
                    balance,
                    1_000_000_000 + currency,
                    BalanceAdjustmentType.ADJUSTMENT);
            }
        }
    }
}
