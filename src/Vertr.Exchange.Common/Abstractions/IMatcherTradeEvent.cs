using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;
public interface IMatcherTradeEvent
{
    MatcherEventType EventType { get; }

    bool ActiveOrderCompleted { get; }

    bool MatchedOrderCompleted { get; }

    long MatchedOrderId { get; }

    long MatchedOrderUid { get; }

    decimal Price { get; }

    long Size { get; }

    decimal BidderHoldPrice { get; }

    IMatcherTradeEvent? NextEvent { get; set; }
}
