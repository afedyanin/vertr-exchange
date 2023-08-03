using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class CancelOrderCommand : OrderBookCommand
{
    public CancelOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var order = OrderBook.GetOrder(OrderCommand.OrderId);

        if (order is null)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        if (order.Uid != OrderCommand.Uid)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        OrderBook.RemoveOrder(order);
        OrderCommand.AttachReduceEvent(order, order.Remaining, true);

        // fill action fields (for events handling)
        // TODO: How and where is it used?
        OrderCommand.Action = order.Action;

        return CommandResultCode.SUCCESS;
    }
}
