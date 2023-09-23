using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;

public class CancelOrderCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.CANCEL_ORDER;

    public long Uid { get; }

    public int Symbol { get; }

    public CancelOrderCommand(
        long orderId,
        DateTime timestamp,
        long uid,
        int symbol) : base(orderId, timestamp)
    {
        Uid = uid;
        Symbol = symbol;
    }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Symbol;
    }
}
