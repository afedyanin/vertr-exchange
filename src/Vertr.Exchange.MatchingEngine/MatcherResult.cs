using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class MatcherResult
{
    public long Volume { get; }

    public long[] OrdersToRemove { get; }

    public IMatcherTradeEvent[] TradeEvents { get; }

    public MatcherResult(
        long volume,
        long[] ordersToRemove,
        IMatcherTradeEvent[] tradeEvents)
    {
        Volume = volume;
        OrdersToRemove = ordersToRemove;
        TradeEvents = tradeEvents;
    }
}
