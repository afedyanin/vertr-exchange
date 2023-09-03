using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal sealed class NewIocOrderCommand : OrderBookCommand
{
    public NewIocOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
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

        var rejectedSize = OrderCommand.Size - result.Filled;

        if (rejectedSize != 0L)
        {
            // was not matched completely - send reject for not-completed IoC order
            EventsHelper.AttachRejectEvent(OrderCommand, OrderCommand.Price, rejectedSize);
        }

        return CommandResultCode.SUCCESS;
    }
}
