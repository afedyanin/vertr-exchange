namespace Vertr.ExchCore.Domain.ValueObjects;

public record class CommandExecutionResult
{
    public int Symbol { get; set; }

    public long Volume { get; set; }

    public long Price { get; set; }

    public long OrderId { get; set; }

    public long Uuid { get; set; }

    public long Timestamp { get; set; }
}
