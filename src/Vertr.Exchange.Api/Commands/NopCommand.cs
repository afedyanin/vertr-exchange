using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;

public class NopCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.NOP;

    public NopCommand(
        long orderId,
        DateTime timestamp) : base(orderId, timestamp)
    {
    }
}
