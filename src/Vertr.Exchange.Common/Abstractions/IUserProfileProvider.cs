using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;

public interface IUserProfileProvider
{
    IUserProfile? Get(long uid);

    public IUserProfile GetOrAdd(long uid, UserStatus status);

    void BatchAdd(IDictionary<int, IDictionary<int, decimal>> users);

    void Reset();
}
