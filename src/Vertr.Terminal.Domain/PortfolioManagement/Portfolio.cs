using System.Collections.Concurrent;

namespace Vertr.Terminal.Domain.PortfolioManagement;

public class Portfolio(long uid)
{
    private readonly ConcurrentDictionary<int, Position> _positions = [];

    public long Uid { get; } = uid;

    public Position? GetPosition(int symbol)
    {
        _positions.TryGetValue(symbol, out var position);

        return position;
    }

    public void ApplyTrade(PositionTrade positionTrade)
    {
        ValidateTrade(positionTrade);

        var symbol = positionTrade.Symbol;
        var position = _positions.GetOrAdd(symbol, new Position(Uid, symbol));

        position.AddTrade(positionTrade);
    }

    public void Reset()
    {
        _positions.Clear();
    }

    private void ValidateTrade(PositionTrade trade)
    {
        ArgumentNullException.ThrowIfNull(trade);

        if (trade.Uid != Uid)
        {
            throw new ArgumentException($"Invalid Trade Uid. Portfolio Uid={Uid}, Trade Uid={trade.Uid}");
        }
    }
}
