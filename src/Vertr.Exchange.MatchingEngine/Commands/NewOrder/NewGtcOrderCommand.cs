using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal sealed class NewGtcOrderCommand : OrderBookCommand
{
    public NewGtcOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        if (!OrderCommand.Action.HasValue)
        {
            return CommandResultCode.DROP; // Invalid OrderAction
        }

        var result = OrderBook.TryMatchInstantly(
            OrderCommand.Action.Value,
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
            OrderCommand.Action.Value,
            OrderCommand.OrderId,
            OrderCommand.Price,
            OrderCommand.Size,
            result.Filled,
            OrderCommand.Uid,
            OrderCommand.Timestamp);

        if (!OrderBook.AddOrder(order))
        {
            // duplicate order id - can match, but can not place
            EventsHelper.AttachRejectEvent(OrderCommand, order.Price, order.Remaining);
        }

        return CommandResultCode.SUCCESS;
    }
}
