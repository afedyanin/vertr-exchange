namespace Vertr.ExchCore.Domain.ValueObjects;

public record class OrderBook
{
    public int Symbol { get; set; }

    public IList<OrderBookRecord>? Asks { get; set; }

    public IList<OrderBookRecord>? Bids { get; set; }

    public long Timestamp { get; set; }
}
