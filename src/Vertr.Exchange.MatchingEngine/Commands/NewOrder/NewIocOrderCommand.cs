using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Helpers;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal class NewIocOrderCommand : OrderBookCommand
{
    public NewIocOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var filledSize = OrderBook.TryMatchInstantly(OrderCommand);
        var rejectedSize = OrderCommand.Size - filledSize;

        if (rejectedSize != 0L)
        {
            // was not matched completely - send reject for not-completed IoC order
            OrderCommand.AttachRejectEvent(OrderCommand.Price, rejectedSize);
        }

        return CommandResultCode.SUCCESS;
    }
}
