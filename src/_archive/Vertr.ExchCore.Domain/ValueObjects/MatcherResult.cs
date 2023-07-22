namespace Vertr.ExchCore.Domain.ValueObjects;

public class MatcherResult
{
    public MatcherTradeEvent EventsChainHead { get; }
    public MatcherTradeEvent EventsChainTail { get; }
    public long Volume { get; }
    public List<long> OrdersToRemove { get; }

    public MatcherResult(
        MatcherTradeEvent eventsChainHead,
        MatcherTradeEvent eventsChainTail,
        long volume,
        List<long> ordersToRemove)
    {
        EventsChainHead = eventsChainHead;
        EventsChainTail = eventsChainTail;
        Volume = volume;
        OrdersToRemove = ordersToRemove;
    }
}
