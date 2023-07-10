using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.ValueObjects;

public record class MatcherTradeEvent
{
    public MatcherEventType EventType { get; set; }

    public int Section { get; set; }

    public bool ActiveOrderCompleted { get; set; }

    public long MatcherOrderId { get; set; }

    public long MatcherOrderUid { get; set; }

    public bool MathcerOrderCompleted { get; set; }

    public long Price { get; set; }

    public long Size { get; set; }

    public long BidderHoldPrice { get; set; }

    public MatcherTradeEvent? NextEvent { get; set; } 
}
