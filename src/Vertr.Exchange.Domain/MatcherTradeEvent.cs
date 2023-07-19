namespace Vertr.Exchange.Domain;

public class MatcherTradeEvent
{
    public MatcherEventType EventType { get; set; }

    public bool ActiveOrderCompleted { get; set; }

    public bool MatchedOrderCompleted { get; set; }

    public long MatchedOrderId { get; set; }

    public long MatchedOrderUid { get; set; }

    public long Price { get; set; }

    public long Size { get; set; }

    public MatcherTradeEvent? NextEvent { get; set; }
}
