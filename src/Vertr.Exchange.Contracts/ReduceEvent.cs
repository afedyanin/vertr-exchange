namespace Vertr.Exchange.Contracts;

public record ReduceEvent
{
    public int Symbol { get; set; }

    public long ReducedVolume { get; set; }

    public bool OrderCompleted { get; set; }

    public decimal Price { get; set; }

    public long OrderId { get; set; }

    public long Uid { get; set; }

    public DateTime Timestamp { get; set; }

    public long Seq { get; set; }
}
