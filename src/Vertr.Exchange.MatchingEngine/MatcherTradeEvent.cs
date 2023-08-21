using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class MatcherTradeEvent : IMatcherTradeEvent
{
    public MatcherEventType EventType { get; set; }

    public bool ActiveOrderCompleted { get; set; }

    public bool MatchedOrderCompleted { get; set; }

    public long MatchedOrderId { get; set; }

    public long MatchedOrderUid { get; set; }

    public decimal Price { get; set; }

    public decimal BidderHoldPrice { get; set; }

    public long Size { get; set; }

    public IMatcherTradeEvent? NextEvent { get; set; }
}
