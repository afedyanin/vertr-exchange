using Vertr.Exchange.Common;

namespace Vertr.Exchange.Api.Commands;
public class AddUserCommand : ApiCommandBase
{
    public AddUserCommand(long orderId, DateTime timestamp) : base(orderId, timestamp)
    {
    }

    public long Uid { get; set; }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);
        command.Command = Common.Enums.OrderCommandType.ADD_USER;
        command.Uid = Uid;
    }
}
