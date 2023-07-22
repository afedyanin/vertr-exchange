namespace Vertr.ExchCore.Domain.Events.OrderEvents;

public record class ReduceOrderEvent : OrderCommandEvent
{
    public long OrderId { get; set; }

    public long Uiid { get; set; }

    public int Symbol { get; set; }

    public long ReduceSize { get; set; }
}
