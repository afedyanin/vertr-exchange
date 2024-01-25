using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.Accounts;
internal class UserProfile(long uid, UserStatus status) : IUserProfile
{
    // currency
    private readonly Dictionary<int, decimal> _accounts = [];

    // symbol
    private readonly Dictionary<int, Position> _positions = [];

    public long Uid { get; } = uid;

    public UserStatus Status { get; private set; } = status;

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
        _accounts.TryAdd(currency, decimal.Zero);
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
        if (!_positions.TryGetValue(spec.SymbolId, out var value))
        {
            value = new Position(Uid, spec.SymbolId);
            _positions.Add(spec.SymbolId, value);
        }

        var position = value;
        position.Update(action, tradeSize, tradePrice);

        // TODO: how to sync account value with each position moving: open, close, change?
        /*if (position.IsEmpty)
        {
            AddToValue(spec.Currency, position.RealizedPnL);
            _positions.Remove(position.Symbol);
        }*/
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

        if (Status == UserStatus.SUSPENDED)
        {
            return CommandResultCode.USER_MGMT_USER_ALREADY_SUSPENDED;
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

    private static Dictionary<int, decimal> GetSnapshot(Dictionary<int, decimal> dict)
    {
        var res = new Dictionary<int, decimal>();

        foreach (var kvp in dict)
        {
            res.Add(kvp.Key, kvp.Value);
        }

        return res;
    }

    private static Dictionary<int, IPosition> GetSnapshot(Dictionary<int, Position> dict)
    {
        var res = new Dictionary<int, IPosition>();

        foreach (var kvp in dict)
        {
            res.Add(kvp.Key, kvp.Value);
        }

        return res;
    }
}
