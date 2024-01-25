using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.Common.Abstractions;

public interface IUserProfileProvider
{
    IUserProfile? Get(long uid);

    public IUserProfile GetOrAdd(long uid, UserStatus status);

    CommandResultCode BatchAdd(IDictionary<long, IDictionary<int, decimal>> users);

    void Reset();
}
