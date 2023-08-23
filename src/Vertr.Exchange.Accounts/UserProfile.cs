using Vertr.Exchange.Accounts.Abstractions;
using Vertr.Exchange.Accounts.Enums;
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

    public decimal? GetValue(int currency)
        => _accounts.ContainsKey(currency) ? _accounts[currency] : null;

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

    public Position? GetPosition(int symbol)
    {
        _positions.TryGetValue(symbol, out var currentPosition);
        return currentPosition;
    }

    public void UpdatePosition(
        int symbol,
        OrderAction action,
        long tradeSize,
        decimal tradePrice,
        int currency)
    {
        if (!_positions.ContainsKey(symbol))
        {
            _positions.Add(symbol, new Position(Uid, symbol, currency));
        }

        var position = _positions[symbol];
        position.Update(action, tradeSize, tradePrice);

        if (position.IsEmpty)
        {
            AddToValue(position.Currency, position.RealizedPnL);
            _positions.Remove(position.Symbol);
        }
    }

    public bool Suspend()
    {
        if (HasPositions || HasAccounts)
        {
            return false;

        }

        Status = UserStatus.SUSPENDED;
        return true;
    }

    public bool Resume()
    {
        if (Status != UserStatus.SUSPENDED)
        {
            return false;
        }

        Status = UserStatus.ACTIVE;
        return true;
    }
}
