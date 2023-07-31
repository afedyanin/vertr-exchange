using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal class NewIocOrderCommand : NewOrderCommand
{
    public NewIocOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var filledSize = OrderBook.TryMatchInstantly(OrderCommand, 0L, OrderCommand);

        var rejectedSize = OrderCommand.Size - filledSize;

        if (rejectedSize != 0L)
        {
            // was not matched completely - send reject for not-completed IoC order
            OrderCommand.MatcherEvent = CreateRejectEvent(OrderCommand.Price, rejectedSize);
        }

        return CommandResultCode.SUCCESS;
    }
}
