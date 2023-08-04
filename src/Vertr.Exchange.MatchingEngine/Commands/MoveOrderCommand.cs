using System.Diagnostics;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal sealed class MoveOrderCommand : OrderBookCommand
{
    public MoveOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
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

        // try match with new price
        var filled = OrderBook.TryMatchInstantly(OrderCommand, Order.Filled);

        if (filled == Order.Size)
        {
            return CommandResultCode.SUCCESS;
        }

        // if not filled completely - put it into corresponding bucket
        Order.SetPrice(OrderCommand.Price);
        Order.Fill(filled - Order.Filled);
        OrderBook.UpdateOrder(Order);

        return CommandResultCode.SUCCESS;

    }
}
