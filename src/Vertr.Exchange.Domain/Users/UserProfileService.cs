using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain.Users;

internal class UserProfileService : IUserProfileService
{
    private readonly IDictionary<long, UserProfile> _userProfiles;

    public UserProfileService()
    {
        _userProfiles = new Dictionary<long, UserProfile>();
    }

    public bool AddEmptyUserProfile(long uid)
    {
        throw new NotImplementedException();
    }

    public CommandResultCode BalanceAdjustment(long uid, int symbol, long amount, long fundingTransactionId)
    {
        throw new NotImplementedException();
    }

    public UserProfile? GetUserProfile(long uid)
    {
        return _userProfiles.TryGetValue(uid, out var userProfile) ? userProfile : null;
    }

    public UserProfile GetUserProfileOrAddSuspended(long uid)
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public CommandResultCode ResumeUserProfile(long uid)
    {
        throw new NotImplementedException();
    }

    public CommandResultCode SuspendUserProfile(long uid)
    {
        throw new NotImplementedException();
    }
}
