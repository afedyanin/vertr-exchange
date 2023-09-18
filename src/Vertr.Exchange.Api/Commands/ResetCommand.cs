using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public class ResetCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.RESET;

    public ResetCommand(
        long orderId,
        DateTime timestamp) : base(orderId, timestamp)
    {
    }
}
