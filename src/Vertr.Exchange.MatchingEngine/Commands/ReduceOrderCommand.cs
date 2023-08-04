using System.Diagnostics;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class ReduceOrderCommand : OrderBookCommand
{
    public ReduceOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

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

        UpdateCommandAction();

        var reduced = OrderBook.Reduce(Order, requestedReduceSize);
        OrderCommand.AttachReduceEvent(Order, reduced, Order.Completed);

        return CommandResultCode.SUCCESS;
    }
}
