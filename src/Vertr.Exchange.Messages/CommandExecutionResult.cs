namespace Vertr.Exchange.Messages;

public record class CommandExecutionResult
{
    public int Symbol { get; init; }
    public long Volume { get; init; }
    public decimal Price { get; init; }
    public long OrderId { get; init; }
    public long Uid { get; init; }
    public DateTime Timestamp { get; init; }
}
