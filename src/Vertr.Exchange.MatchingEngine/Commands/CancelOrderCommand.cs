using System.Diagnostics;
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
        if (!HasValidOrder)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        Debug.Assert(Order is not null);

        UpdateCommandAction();

        OrderBook.RemoveOrder(Order);

        OrderCommand.AttachReduceEvent(Order, Order.Remaining, true);

        return CommandResultCode.SUCCESS;
    }
}
