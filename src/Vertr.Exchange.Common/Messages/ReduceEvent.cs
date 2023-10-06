namespace Vertr.Exchange.Common.Messages;
public record class ReduceEvent
{
    public int Symbol { get; init; }
    public long ReducedVolume { get; init; }
    public bool OrderCompleted { get; init; }
    public decimal Price { get; init; }
    public long OrderId { get; init; }
    public long Uid { get; init; }
    public DateTime Timestamp { get; init; }
    public long Seq { get; init; }
}
