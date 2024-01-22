namespace Vertr.Terminal.Domain.PortfolioManagement;

public class Position(long uid, int symbol)
{
    private readonly List<PositionTrade> _trades = [];

    private readonly List<PositionPnlRecord> _pnlHistory = [];

    public long Uid { get; } = uid;

    public int Symbol { get; } = symbol;

    public PositionTrade[] Trades => _trades.ToArray();

    public PositionPnlRecord[] PnlHistory => _pnlHistory.ToArray();

    public PositionPnlRecord Pnl { get; private set; } = new PositionPnlRecord();

    public void Reset()
    {
        _trades.Clear();
    }

    public void AddTrade(PositionTrade trade)
    {
        ValidateTrade(trade);
        _trades.Add(trade);
        var newPnl = Pnl.ApplyTrade(trade);
        _pnlHistory.Add(newPnl);
        Pnl = newPnl;
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
