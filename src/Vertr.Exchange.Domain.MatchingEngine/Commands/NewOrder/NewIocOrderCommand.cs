using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Events;

namespace Vertr.Exchange.Domain.MatchingEngine.Commands.NewOrder;
internal sealed class NewIocOrderCommand(
    IOrderBook orderBook,
    OrderCommand cmd)
    : OrderBookCommand(orderBook, cmd)
{
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

        var rejectedSize = OrderCommand.Size - result.Filled;

        if (rejectedSize != 0L)
        {
            // was not matched completely - send reject for not-completed IoC order
            EventsHelper.AttachRejectEvent(OrderCommand, OrderCommand.Price, rejectedSize);
        }

        return CommandResultCode.SUCCESS;
    }
}
