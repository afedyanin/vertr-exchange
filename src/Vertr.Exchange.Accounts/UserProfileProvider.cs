using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts;

internal class UserProfileProvider : IUserProfileProvider
{
    // uid
    private readonly IDictionary<long, IUserProfile> _userProfiles;

    public UserProfileProvider()
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

    public void BatchAdd(IDictionary<int, IDictionary<int, decimal>> users)
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
