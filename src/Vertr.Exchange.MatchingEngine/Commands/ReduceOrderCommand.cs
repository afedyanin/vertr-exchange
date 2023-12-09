using System.Diagnostics;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class ReduceOrderCommand(
    IOrderBook orderBook,
    OrderCommand cmd)
    : OrderBookCommand(orderBook, cmd)
{
    public override CommandResultCode Execute()
    {
        var requestedReduceSize = OrderCommand.Size;

        if (requestedReduceSize <= 0L)
        {
            return CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE;
        }

        if (!HasValidOrder)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        Debug.Assert(Order is not null);

        var reduced = OrderBook.Reduce(Order, requestedReduceSize);
        EventsHelper.AttachReduceEvent(OrderCommand, Order, reduced, Order.Completed);

        return CommandResultCode.SUCCESS;
    }
}
