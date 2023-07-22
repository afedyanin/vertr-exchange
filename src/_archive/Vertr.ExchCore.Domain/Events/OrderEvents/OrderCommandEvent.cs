using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.Events.OrderEvents;

public record class OrderCommandEvent
{
    public DateTime Timestamp { get; set; }

    public CommandResultCode ResultCode { get; }

    public long Sequence { get; }
}
