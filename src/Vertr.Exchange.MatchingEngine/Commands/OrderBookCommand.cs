using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;

internal abstract class OrderBookCommand
{
    protected OrderCommand OrderCommand { get; }

    protected IOrderBook OrderBook { get; }

    protected OrderBookCommand(
        IOrderBook orderBook,
        OrderCommand cmd)
    {
        OrderBook = orderBook;
        OrderCommand = cmd;
    }
    public abstract CommandResultCode Execute();

    protected IMatcherTradeEvent CreateReduceEvent(
        IOrder order,
        long reduceSize,
        bool completed)
    {
        return new MatcherTradeEvent
        {
            EventType = MatcherEventType.REDUCE,
            ActiveOrderCompleted = completed,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = order.Price,
            Size = reduceSize,
        };
    }

    protected IMatcherTradeEvent CreateRejectEvent(
        long price,
        long rejectedSize)
    {
        return new MatcherTradeEvent
        {
            EventType = MatcherEventType.REJECT,
            ActiveOrderCompleted = true,
            MatchedOrderId = 0L,
            MatchedOrderCompleted = false,
            Price = price,
            Size = rejectedSize,
        };
    }
}
