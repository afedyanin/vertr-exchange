using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain.Users;
internal sealed class UserProfile
{
    public long Uid { get; }

    public UserStatus UserStatus { get; }

    public IDictionary<int, long> Accounts { get; } = new Dictionary<int, long>();

    public IDictionary<int, SymbolPositionRecord> Positions { get; } = new Dictionary<int, SymbolPositionRecord>();

    public long AdjustmentsCounter { get; }
}
