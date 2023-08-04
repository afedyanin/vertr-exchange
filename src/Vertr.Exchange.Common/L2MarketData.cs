namespace Vertr.Exchange.Common;

public class L2MarketData
{
    public int AskSize { get; set; }

    public int BidSize { get; set; }

    public decimal[] AskPrices { get; set; } = Array.Empty<decimal>();

    public long[] AskVolumes { get; set; } = Array.Empty<long>();

    public long[] AskOrders { get; set; } = Array.Empty<long>();

    public decimal[] BidPrices { get; set; } = Array.Empty<decimal>();

    public long[] BidVolumes { get; set; } = Array.Empty<long>();

    public long[] BidOrders { get; set; } = Array.Empty<long>();

    public DateTime Timestamp { get; set; }

    public long ReferenceSeq { get; set; }

    public L2MarketData(int askSize, int bidSize)
    {
        AskSize = askSize;
        BidSize = bidSize;
    }
}
