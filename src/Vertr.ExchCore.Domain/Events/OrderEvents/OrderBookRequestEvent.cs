namespace Vertr.ExchCore.Domain.Events.OrderEvents;

public record class OrderBookRequestEvent
{
    public int Symbol { get; set; }

    public int Size { get; set; }
}
