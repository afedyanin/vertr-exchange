using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Commands;
public class ResumeUserCommand(
    long orderId,
    DateTime timestamp,
    long uid)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.RESUME_USER;

    public long Uid { get; } = uid;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
    }
}
