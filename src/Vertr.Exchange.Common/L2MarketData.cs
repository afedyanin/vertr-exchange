namespace Vertr.Exchange.Domain.Common;

public class L2MarketData(int askSize, int bidSize, long refSeq)
{
    public int AskSize { get; set; } = askSize;

    public int BidSize { get; set; } = bidSize;

    public decimal[] AskPrices { get; set; } = [];

    public long[] AskVolumes { get; set; } = [];

    public long[] AskOrders { get; set; } = [];

    public decimal[] BidPrices { get; set; } = [];

    public long[] BidVolumes { get; set; } = [];

    public long[] BidOrders { get; set; } = [];

    public long ReferenceSeq { get; set; } = refSeq;
}
