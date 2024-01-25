using System.Diagnostics;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.MatchingEngine.Commands;
internal sealed class MoveOrderCommand(
    IOrderBook orderBook,
    OrderCommand cmd)
    : OrderBookCommand(orderBook, cmd)
{
    public override CommandResultCode Execute()
    {
        if (!HasValidOrder)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        Debug.Assert(Order is not null);

        var removed = OrderBook.RemoveOrder(Order);

        if (!removed)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        // try match with new price
        var result = OrderBook.TryMatchInstantly(
            Order.Action,
            OrderCommand.Price,
            Order.Size,
            Order.Filled);

        AttachTradeEvents(result.TradeEvents);

        if (result.Filled == Order.Size)
        {
            return CommandResultCode.SUCCESS;
        }

        // if not filled completely - put it into corresponding bucket
        Order.Update(OrderCommand.Price, Order.Size, result.Filled);
        var added = OrderBook.AddOrder(Order);

        if (!added)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        return CommandResultCode.SUCCESS;
    }
}
