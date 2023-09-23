using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public class ReduceOrderCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.REDUCE_ORDER;

    public long Uid { get; }

    public int Symbol { get; }

    public int ReduceSize { get; }

    public ReduceOrderCommand(
        long orderId,
        DateTime timestamp,
        long uid,
        int symbol,
        int reduceSize) : base(orderId, timestamp)
    {
        Uid = uid;
        Symbol = symbol;
        ReduceSize = reduceSize;
    }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Symbol;
        command.Size = ReduceSize;
    }
}
