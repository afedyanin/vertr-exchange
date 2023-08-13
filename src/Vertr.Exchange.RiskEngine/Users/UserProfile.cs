namespace Vertr.Exchange.RiskEngine.Users;
public sealed class UserProfile
{
    public long Uid { get; }

    public UserStatus UserStatus { get; set; }

    public IDictionary<int, decimal> Accounts { get; } = new Dictionary<int, decimal>();

    public IDictionary<int, SymbolPositionRecord> Positions { get; } =
        new Dictionary<int, SymbolPositionRecord>();

    public long AdjustmentsCounter { get; set; }

    public UserProfile(long uid, UserStatus status)
    {
        Uid = uid;
        UserStatus = status;
    }
}
