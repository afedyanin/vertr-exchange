using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public class AddUserCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.ADD_USER;

    public long Uid { get; }

    public AddUserCommand(
        long orderId,
        DateTime timestamp,
        long uid) : base(orderId, timestamp)
    {
        Uid = uid;
    }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);
        command.Uid = Uid;
    }
}
