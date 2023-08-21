namespace Vertr.Exchange.RiskEngine.Users;

internal sealed class UserProfile
{
    // currency
    private readonly IDictionary<int, decimal> _accounts;

    // symbol
    private readonly IDictionary<int, SymbolPositionRecord> _positions;

    public long Uid { get; }

    public UserStatus UserStatus { get; set; }

    public long AdjustmentsCounter { get; set; }

    public UserProfile(long uid, UserStatus status)
    {
        Uid = uid;
        UserStatus = status;
        _accounts = new Dictionary<int, decimal>();
        _positions = new Dictionary<int, SymbolPositionRecord>();
    }

    public SymbolPositionRecord[] Positions => _positions.Values.ToArray();

    public bool HasPositions => _positions.Values.Any(pos => !pos.IsEmpty());

    public bool HasAccounts => _accounts.Values.Any(acc => acc != 0L);

    public decimal GetCurrentAmount(int currency)
    {
        _accounts.TryGetValue(currency, out var currentAmount);
        return currentAmount;
    }

    public SymbolPositionRecord? GetCurrentPosition(int symbol)
    {
        _positions.TryGetValue(symbol, out var currentPosition);
        return currentPosition;
    }

    public SymbolPositionRecord? GetPositionByCurrency(int currency)
    {
        return _positions.Values.FirstOrDefault(p => p.Currency == currency);
    }

    public decimal AddToValue(int symbol, decimal toBeAdded)
    {
        if (!_accounts.ContainsKey(symbol))
        {
            _accounts.Add(symbol, toBeAdded);
            return toBeAdded;
        }
        else
        {
            _accounts[symbol] += toBeAdded;
            return _accounts[symbol];
        }
    }

    public void RemovePositionRecord(SymbolPositionRecord record)
    {
        AddToValue(record.Currency, record.Profit);
        _positions.Remove(record.Symbol);
    }
}
