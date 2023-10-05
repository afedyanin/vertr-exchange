namespace Vertr.Exchange.Common.Messages;
public record class OrderBook
{
    public int Symbol { get; init; }

    public IEnumerable<OrderBookRecord> Asks { get; init; } = Enumerable.Empty<OrderBookRecord>();
    public IEnumerable<OrderBookRecord> Bids { get; init; } = Enumerable.Empty<OrderBookRecord>();

    public DateTime Timestamp { get; init; }
}
