using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;
public interface IMatcherTradeEvent
{
    MatcherEventType EventType { get; }

    bool ActiveOrderCompleted { get; }

    bool MatchedOrderCompleted { get; }

    long MatchedOrderId { get; }

    long MatchedOrderUid { get; }

    long Price { get; }

    long Size { get; }

    IMatcherTradeEvent? NextEvent { get; }
}
