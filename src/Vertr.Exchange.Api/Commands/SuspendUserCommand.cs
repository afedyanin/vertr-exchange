using Vertr.Exchange.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Commands;
public class SuspendUserCommand(
    long orderId,
    DateTime timestamp,
    long uid)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.SUSPEND_USER;
    public long Uid { get; } = uid;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
    }
}
