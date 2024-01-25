using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Commands;
public class MoveOrderCommand(
    long orderId,
    DateTime timestamp,
    long uid,
    decimal newPrice,
    int symbol)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.MOVE_ORDER;

    public long Uid { get; } = uid;

    public int Symbol { get; } = symbol;

    public decimal NewPrice { get; } = newPrice;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Symbol;
        command.Price = NewPrice;
    }
}
