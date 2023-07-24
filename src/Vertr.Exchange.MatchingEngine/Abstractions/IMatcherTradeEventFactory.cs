using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.MatchingEngine.Abstractions;
internal interface IMatcherTradeEventFactory
{
    IMatcherTradeEvent CreateTradeEvent(
        IOrder matchingOrder,
        bool makerCompleted,
        bool takerCompleted,
        long size);

    IMatcherTradeEvent CreateReduceEvent(
        IOrder order,
        long reduceSize,
        bool completed);

    IMatcherTradeEvent CreateRejectEvent(
    long price,
    long rejectedSize);
}
