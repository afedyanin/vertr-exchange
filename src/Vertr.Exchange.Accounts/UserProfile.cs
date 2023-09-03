using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts;
internal class UserProfile : IUserProfile
{
    // currency
    private readonly IDictionary<int, decimal> _accounts;

    // symbol
    private readonly IDictionary<int, Position> _positions;

    public long Uid { get; }

    public UserStatus Status { get; private set; }

    public UserProfile(long uid, UserStatus status)
    {
        Uid = uid;
        Status = status;
        _accounts = new Dictionary<int, decimal>();
        _positions = new Dictionary<int, Position>();
    }

    public IDictionary<int, decimal> Accounts
        => GetSnapshot(_accounts);

    public IDictionary<int, IPosition> Positions
        => GetSnapshot(_positions);

    public decimal? GetValue(int currency)
    {
        if (_accounts.TryGetValue(currency, out var value))
        {
            return value;
        }

        return null;
    }

    public bool HasAccounts => _accounts.Values.Any(acc => acc != decimal.Zero);

    public bool HasPositions => _positions.Values.Any(pos => !pos.IsEmpty);

    public decimal AddToValue(int currency, decimal toBeAdded)
    {
        if (!_accounts.ContainsKey(currency))
        {
            _accounts.Add(currency, decimal.Zero);
        }

        _accounts[currency] += toBeAdded;
        return _accounts[currency];
    }

    public IPosition? GetPosition(int symbol)
    {
        _positions.TryGetValue(symbol, out var currentPosition);
        return currentPosition;
    }

    public void UpdatePosition(
        SymbolSpecification spec,
        OrderAction action,
        long tradeSize,
        decimal tradePrice)
    {
        if (!_positions.ContainsKey(spec.SymbolId))
        {
            _positions.Add(spec.SymbolId, new Position(Uid, spec.SymbolId));
        }

        var position = _positions[spec.SymbolId];
        position.Update(action, tradeSize, tradePrice);

        if (position.IsEmpty)
        {
            AddToValue(spec.Currency, position.RealizedPnL);
            _positions.Remove(position.Symbol);
        }
    }

    public CommandResultCode Suspend()
    {
        if (HasPositions)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_HAS_POSITIONS;
        }

        if (HasAccounts)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_SUSPENDABLE_NON_EMPTY_ACCOUNTS;

        }

        Status = UserStatus.SUSPENDED;
        return CommandResultCode.SUCCESS;
    }

    public CommandResultCode Resume()
    {
        if (Status != UserStatus.SUSPENDED)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_SUSPENDED;
        }

        Status = UserStatus.ACTIVE;
        return CommandResultCode.SUCCESS;
    }

    private static IDictionary<int, decimal> GetSnapshot(IDictionary<int, decimal> dict)
    {
        var res = new Dictionary<int, decimal>();

        foreach (var kvp in dict)
        {
            res.Add(kvp.Key, kvp.Value);
        }

        return res;
    }

    private static IDictionary<int, IPosition> GetSnapshot(IDictionary<int, Position> dict)
    {
        var res = new Dictionary<int, IPosition>();

        foreach (var kvp in dict)
        {
            res.Add(kvp.Key, kvp.Value);
        }

        return res;
    }
}
