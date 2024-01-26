using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Commands;
public class ReduceOrderCommand(
    long orderId,
    DateTime timestamp,
    long uid,
    int symbol,
    long reduceSize)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.REDUCE_ORDER;

    public long Uid { get; } = uid;

    public int Symbol { get; } = symbol;

    public long ReduceSize { get; } = reduceSize;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Symbol;
        command.Size = ReduceSize;
    }
}
