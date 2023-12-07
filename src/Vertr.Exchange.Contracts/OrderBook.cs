namespace Vertr.Exchange.Contracts;

public record OrderBook
{
    public int Symbol { get; set; }

    public OrderBookRecord[] Asks { get; set; } = [];

    public OrderBookRecord[] Bids { get; set; } = [];

    public DateTime Timestamp { get; set; }

    public long Seq { get; set; }
}
