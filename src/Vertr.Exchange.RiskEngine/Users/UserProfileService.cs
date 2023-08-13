using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Users;

internal class UserProfileService : IUserProfileService
{
    private readonly IDictionary<long, UserProfile> _userProfiles;

    public UserProfileService()
    {
        _userProfiles = new Dictionary<long, UserProfile>();
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
        else if (profile.Positions.Values.Any(pos => !pos.IsEmpty()))
        {
            return CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS;

        }
        else if (profile.Accounts.Values.Any(acc => acc != 0L))
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
    }

    public CommandResultCode BalanceAdjustment(
        long uid,
        int symbol,
        decimal amount,
        long fundingTransactionId)
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
        profile.Accounts.TryGetValue(symbol, out var currentAmount);

        if (amount < 0 && (currentAmount + amount) < 0)
        {
            return CommandResultCode.USER_MGMT_ACCOUNT_BALANCE_ADJUSTMENT_NSF;
        }

        profile.AdjustmentsCounter = fundingTransactionId;

        if (!profile.Accounts.ContainsKey(symbol))
        {
            profile.Accounts.Add(symbol, amount);
        }
        else
        {
            profile.Accounts[symbol] += amount;
        }

        //log.debug("FUND: {}", userProfile);
        return CommandResultCode.SUCCESS;
    }
}
