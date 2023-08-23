using Vertr.Exchange.Accounts.Enums;

namespace Vertr.Exchange.Accounts.Abstractions;

public interface IUserProfilesRepository
{
    IUserProfile? Get(long uid);

    public IUserProfile GetOrAdd(long uid, UserStatus status);

    void BatchAdd(IDictionary<int, IDictionary<int, long>> users);

    void Reset();
}
