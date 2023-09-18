using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public class MoveOrderCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.MOVE_ORDER;

    public long Uid { get; }

    public int Symbol { get; }

    public decimal NewPrice { get; }

    public MoveOrderCommand(
        long orderId,
        DateTime timestamp,
        long uid,
        decimal newPrice,
        int symbol) : base(orderId, timestamp)
    {
        Uid = uid;
        NewPrice = newPrice;
        Symbol = symbol;
    }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Symbol;
        command.Price = NewPrice;
    }
}
