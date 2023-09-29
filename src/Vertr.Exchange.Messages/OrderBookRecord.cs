namespace Vertr.Exchange.Messages;

public record class OrderBookRecord
{
    public decimal Price { get; init; }
    public long Volume { get; init; }
    public long Orders { get; init; }
}

