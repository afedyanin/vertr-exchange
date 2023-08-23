using Vertr.Exchange.Accounts.Abstractions;
using Vertr.Exchange.Accounts.Enums;

namespace Vertr.Exchange.Accounts;

internal class UserProfilesRepository : IUserProfilesRepository
{
    // uid
    private readonly IDictionary<long, IUserProfile> _userProfiles;

    public UserProfilesRepository()
    {
        _userProfiles = new Dictionary<long, IUserProfile>();
    }

    public IUserProfile? Get(long uid)
    {
        return _userProfiles.TryGetValue(uid, out var profile) ? profile : null;
    }

    public IUserProfile GetOrAdd(long uid, UserStatus status)
    {
        var profile = Get(uid);

        if (profile is null)
        {
            profile = new UserProfile(uid, status);
            _userProfiles.Add(uid, profile);
        }

        return profile;
    }

    public void Reset()
    {
        _userProfiles.Clear();
    }

    public void BatchAdd(IDictionary<int, IDictionary<int, long>> users)
    {
        foreach (var (uid, accounts) in users)
        {
            var profile = GetOrAdd(uid, UserStatus.ACTIVE);

            foreach (var (currency, balance) in accounts)
            {
                profile.AddToValue(currency, balance);
            }
        }
    }
}
