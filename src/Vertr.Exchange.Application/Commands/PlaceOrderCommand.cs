using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Commands;
public class PlaceOrderCommand(
    long orderId,
    DateTime timestamp,
    decimal price,
    long size,
    OrderAction action,
    OrderType orderType,
    long uid,
    int symbol)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.PLACE_ORDER;

    public decimal Price { get; } = price;

    public long Size { get; } = size;

    public OrderAction Action { get; } = action;

    public OrderType OrderType { get; } = orderType;

    public long Uid { get; } = uid;

    public int Symbol { get; } = symbol;

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
