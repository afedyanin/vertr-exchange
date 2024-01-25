using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Commands;

public class CancelOrderCommand(
    long orderId,
    DateTime timestamp,
    long uid,
    int symbol)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.CANCEL_ORDER;

    public long Uid { get; } = uid;

    public int Symbol { get; } = symbol;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Symbol;
    }
}
