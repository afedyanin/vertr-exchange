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

        OrderBook.RemoveOrder(Order);

        // try match with new price
        var result = OrderBook.TryMatchInstantly(
            OrderCommand.Action,
            OrderCommand.Price,
            OrderCommand.Size,
            Order.Filled);

        UpdateCommandAction();
        AttachTradeEvents(result.TradeEvents);

        if (result.Filled == Order.Size)
        {
            return CommandResultCode.SUCCESS;
        }

        // if not filled completely - put it into corresponding bucket
        Order.SetPrice(OrderCommand.Price);
        Order.Fill(result.Filled - Order.Filled);
        var added = OrderBook.AddOrder(Order);

        return added ? CommandResultCode.SUCCESS : CommandResultCode.MATCHING_INVALID_ORDER_BOOK_ID; // TODO: Return ErrorResultCode
    }
}
