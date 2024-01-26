using System.Diagnostics;
using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Events;

namespace Vertr.Exchange.Domain.MatchingEngine.Commands;
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
