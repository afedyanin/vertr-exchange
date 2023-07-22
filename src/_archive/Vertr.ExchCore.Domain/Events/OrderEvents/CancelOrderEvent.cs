namespace Vertr.ExchCore.Domain.Events.OrderEvents;

public record class CancelOrderEvent : OrderCommandEvent
{
    public long OrderId { get; set; }
    public long Uuid { get; set; }
    public int Symbol { get; set; }
}
