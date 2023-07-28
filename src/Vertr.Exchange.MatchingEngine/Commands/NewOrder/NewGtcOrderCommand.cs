using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands.NewOrder;
internal class NewGtcOrderCommand : NewOrderCommand
{
    public NewGtcOrderCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var action = OrderCommand.Action;
        var price = OrderCommand.Price;
        var size = OrderCommand.Size;

        var filledSize = TryMatchInstantly(OrderCommand, 0L, OrderCommand);

        if (filledSize == size)
        {
            // order was matched completely - nothing to place - can just return
            return;
        }

        var newOrderId = OrderCommand.OrderId;
        if (_orders.ContainsKey(newOrderId))
        {
            // duplicate order id - can match, but can not place
            _eventFactory.AttachRejectEvent(OrderCommand, OrderCommand.Size - filledSize);
            //log.warn("duplicate order id: {}", cmd);
            return;
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

        var buckets = GetBucketsByAction(action);

        if (!buckets.ContainsKey(price))
        {
            buckets.Add(price, new OrdersBucket(_eventFactory, price));
        }

        var bucket = buckets[price];
        bucket.Put(orderRecord);

        _orders.Add(newOrderId, orderRecord);

        return CommandResultCode.SUCCESS;
    }
}
