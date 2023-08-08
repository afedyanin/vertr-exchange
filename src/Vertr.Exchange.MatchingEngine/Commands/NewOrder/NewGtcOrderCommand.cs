using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal sealed class NewGtcOrderCommand : OrderBookCommand
{
    public NewGtcOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var result = OrderBook.TryMatchInstantly(
            OrderCommand.Action,
            OrderCommand.Price,
            OrderCommand.Size);

        AttachTradeEvents(result.TradeEvents);

        if (result.Filled == OrderCommand.Size)
        {
            // order was matched completely - nothing to place - can just return
            return CommandResultCode.SUCCESS;
        }

        // normally placing regular GTC limit order
        var order = new Order(
            OrderCommand.Action,
            OrderCommand.OrderId,
            OrderCommand.Price,
            OrderCommand.Size,
            result.Filled,
            OrderCommand.Uid,
            OrderCommand.Timestamp);

        if (!OrderBook.AddOrder(order))
        {
            // duplicate order id - can match, but can not place
            OrderCommand.AttachRejectEvent(order.Price, order.Remaining);
        }

        return CommandResultCode.SUCCESS;
    }
}
