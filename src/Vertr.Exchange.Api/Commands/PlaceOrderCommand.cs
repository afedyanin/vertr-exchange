using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public class PlaceOrderCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.PLACE_ORDER;

    public decimal Price { get; }

    public long Size { get; }

    public OrderAction Action { get; }

    public OrderType OrderType { get; }

    public long Uid { get; }

    public int Symbol { get; }

    public PlaceOrderCommand(
        long orderId,
        DateTime timestamp,
        decimal price,
        long size,
        OrderAction action,
        OrderType orderType,
        long uid,
        int symbol) : base(orderId, timestamp)
    {
        Price = price;
        Size = size;
        Action = action;
        OrderType = orderType;
        Uid = uid;
        Symbol = symbol;
    }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Price = Price;
        command.Size = Size;
        command.Action = Action;
        command.OrderType = OrderType;
        command.Uid = Uid;
        command.Symbol = Symbol;
    }
}
