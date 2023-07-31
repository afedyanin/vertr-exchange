using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal class NewGtcOrderCommand : NewOrderCommand
{
    public NewGtcOrderCommand(IOrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var action = OrderCommand.Action;
        var price = OrderCommand.Price;
        var size = OrderCommand.Size;

        var filledSize = OrderBook.TryMatchInstantly(OrderCommand, 0L, OrderCommand);

        if (filledSize == size)
        {
            // order was matched completely - nothing to place - can just return
            return OrderCommand.ResultCode; // ???
        }

        // normally placing regular GTC limit order
        var orderRecord = new Order
        {
            Action = action,
            OrderId = newOrderId,
            Price = price,
            Size = size,
            Filled = filledSize,
            Uid = cmd.Uid,
            Timestamp = cmd.Timestamp,
        };

        if (!OrderBook.AddNewOrder(orderRecord))
        {
            // duplicate order id - can match, but can not place
            OrderCommand.MatcherEvent = CreateRejectEvent(OrderCommand.Price, OrderCommand.Size - filledSize);
            return CommandResultCode.SUCCESS; // ???
        }

        return CommandResultCode.SUCCESS;
    }
}
