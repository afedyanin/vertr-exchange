using System.Diagnostics;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class CancelOrderCommand(IOrderBook orderBook, OrderCommand cmd) : OrderBookCommand(orderBook, cmd)
{
    public override CommandResultCode Execute()
    {
        if (!HasValidOrder)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        Debug.Assert(Order is not null);

        OrderBook.RemoveOrder(Order);

        EventsHelper.AttachReduceEvent(OrderCommand, Order, Order.Remaining, true);

        return CommandResultCode.SUCCESS;
    }
}
