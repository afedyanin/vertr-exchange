using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class ReduceOrderCommand : OrderBookCommand
{
    public ReduceOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var orderId = OrderCommand.OrderId;
        var requestedReduceSize = OrderCommand.Size;

        if (requestedReduceSize <= 0)
        {
            return CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE;
        }

        var order = OrderBook.GetOrder(orderId);

        if (order is null)
        {
            // already matched, moved or cancelled
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        if (order.Uid != OrderCommand.Uid)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        var reduced = OrderBook.Reduce(order, requestedReduceSize);

        OrderCommand.AttachReduceEvent(order, reduced, order.Completed);

        // ??? fill action fields (for events handling)
        OrderCommand.Action = order.Action;

        return CommandResultCode.SUCCESS;
    }
}
