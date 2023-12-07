namespace Vertr.Exchange.Contracts;
public record Level2MarketData
{
    public int AskSize { get; set; }

    public int BidSize { get; set; }

    public decimal[] AskPrices { get; set; } = [];

    public long[] AskVolumes { get; set; } = [];

    public long[] AskOrders { get; set; } = [];

    public decimal[] BidPrices { get; set; } = [];

    public long[] BidVolumes { get; set; } = [];

    public long[] BidOrders { get; set; } = [];

    public DateTime Timestamp { get; set; }

    public long ReferenceSeq { get; set; }
}
