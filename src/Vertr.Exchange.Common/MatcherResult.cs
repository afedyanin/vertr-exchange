namespace Vertr.Exchange.Common.Abstractions;

public sealed class MatcherResult
{
    public long Filled { get; }

    public IEnumerable<IMatcherTradeEvent> TradeEvents { get; }

    public MatcherResult(long filled, IEnumerable<IMatcherTradeEvent>? tradeEvents = null)
    {
        Filled = filled;
        TradeEvents = tradeEvents ?? Array.Empty<IMatcherTradeEvent>();
    }
}
