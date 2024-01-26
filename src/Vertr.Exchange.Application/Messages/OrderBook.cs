namespace Vertr.Exchange.Application.Messages;
public record class OrderBook
{
    public int Symbol { get; init; }

    public IEnumerable<OrderBookRecord> Asks { get; init; } = Enumerable.Empty<OrderBookRecord>();
    public IEnumerable<OrderBookRecord> Bids { get; init; } = Enumerable.Empty<OrderBookRecord>();

    public DateTime Timestamp { get; init; }
    public long Seq { get; init; }
}
