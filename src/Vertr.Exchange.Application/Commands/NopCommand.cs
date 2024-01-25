using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Commands;

public class NopCommand(
    long orderId,
    DateTime timestamp)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.NOP;
}
