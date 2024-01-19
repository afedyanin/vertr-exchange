using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class BucketMatcherResult(
    long volume,
    long[] ordersToRemove,
    IEngineEvent[] tradeEvents)
{
    public long Volume { get; } = volume;

    public long[] OrdersToRemove { get; } = ordersToRemove;

    public IEngineEvent[] TradeEvents { get; } = tradeEvents;
}
