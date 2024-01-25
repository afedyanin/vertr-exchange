namespace Vertr.Exchange.Domain.Common.Messages;
public record class RejectEvent
{
    public int Symbol { get; init; }
    public long RejectedVolume { get; init; }
    public decimal Price { get; init; }
    public long OrderId { get; init; }
    public long Uid { get; init; }
    public DateTime Timestamp { get; init; }
    public long Seq { get; init; }
}
