using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Commands;
public class ResetCommand(
    long orderId,
    DateTime timestamp)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.RESET;
}
