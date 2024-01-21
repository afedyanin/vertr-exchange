namespace Vertr.Terminal.Domain.PortfolioManagement;

public class Position(long uid, int symbol)
{
    private readonly List<PositionTrade> _trades = [];

    public long Uid { get; } = uid;

    public int Symbol { get; } = symbol;

    public PositionTrade[] Trades => _trades.ToArray();

    public void Reset()
    {
        _trades.Clear();
    }

    public void AddTrade(PositionTrade trade)
    {
        ValidateTrade(trade);
        _trades.Add(trade);
    }

    public void ValidateTrade(PositionTrade trade)
    {
        ArgumentNullException.ThrowIfNull(trade);

        if (trade.Uid != Uid)
        {
            throw new ArgumentException($"Invalid Trade Uid. Position Uid={Uid}, Trade Uid={trade.Uid}");
        }

        if (trade.Symbol != Symbol)
        {
            throw new ArgumentException($"Invalid Trade Symbol. Position Symbol={Symbol}, Trade Symbol={trade.Symbol}");
        }
    }
}
