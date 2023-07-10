namespace Vertr.ExchCore.Domain.Events.TradeEvents;

public record class ReduceEvent
{
    public int Symbol { get; set; }

    public long ReducedVolume { get; set; }

    public bool OrderCompleted { get; set; }

    public long Price { get; set; }

    public long OrderId { get; set; }

    public long Uuid { get; set; }

    public long Timestamp { get; set; }

}
