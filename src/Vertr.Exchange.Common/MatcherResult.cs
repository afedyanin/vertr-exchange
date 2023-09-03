namespace Vertr.Exchange.Common.Abstractions;

public sealed class MatcherResult
{
    public long Filled { get; }

    public IEnumerable<IEngineEvent> TradeEvents { get; }

    public MatcherResult(long filled, IEnumerable<IEngineEvent>? tradeEvents = null)
    {
        Filled = filled;
        TradeEvents = tradeEvents ?? Array.Empty<IEngineEvent>();
    }
}
