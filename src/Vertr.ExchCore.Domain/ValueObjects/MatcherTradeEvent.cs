using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.ValueObjects;

public record class MatcherTradeEvent
{
    public MatcherEventType EventType { get; set; }

    public int Section { get; set; }

    public bool ActiveOrderCompleted { get; set; }

    public long MatchedOrderId { get; set; }

    public long MatchedOrderUid { get; set; }

    public bool MatchedOrderCompleted { get; set; }

    public long Price { get; set; }

    public long Size { get; set; }

    public long BidderHoldPrice { get; set; }

    public MatcherTradeEvent? NextEvent { get; set; } 
}
