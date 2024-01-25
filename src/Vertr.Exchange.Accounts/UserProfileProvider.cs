using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.Accounts;

internal class UserProfileProvider : IUserProfileProvider
{
    // uid
    private readonly Dictionary<long, IUserProfile> _userProfiles;

    public UserProfileProvider()
    {
        _userProfiles = [];
    }

    public IUserProfile? Get(long uid)
    {
        _userProfiles.TryGetValue(uid, out var profile);
        return profile;
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

    public CommandResultCode BatchAdd(IDictionary<long, IDictionary<int, decimal>> users)
    {
        foreach (var (uid, accounts) in users)
        {
            var profile = GetOrAdd(uid, UserStatus.ACTIVE);

            foreach (var (currency, balance) in accounts)
            {
                profile.AddToValue(currency, balance);
            }
        }

        return CommandResultCode.SUCCESS;
    }
}
