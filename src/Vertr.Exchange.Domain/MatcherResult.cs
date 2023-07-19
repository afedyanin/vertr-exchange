namespace Vertr.Exchange.Domain;
internal class MatcherResult
{
    public long Volume { get; }

    public long[] OrdersToRemove { get; }

    public MatcherTradeEvent[] TradeEvents { get; }

    public MatcherResult(
        long volume,
        long[] ordersToRemove,
        MatcherTradeEvent[] tradeEvents)
    {
        Volume = volume;
        OrdersToRemove = ordersToRemove;
        TradeEvents = tradeEvents;
    }
}
