namespace Vertr.ExchCore.Domain.ValueObjects;

public  class L2MarketData
{
    public int AskSize { get; set; }
    
    public int BidSize { get; set; }

    public long[] AskPrices { get; set; } = Array.Empty<long>();

    public long[] AskVolumes { get; set; } = Array.Empty<long>();

    public long[] AskOrders { get; set; } = Array.Empty<long>();

    public long[] BidPrices { get; set; } = Array.Empty<long>();

    public long[] BidVolumes { get; set; } = Array.Empty<long>();

    public long[] BidOrders { get; set; } = Array.Empty<long>();

    public long Timestamp { get; set; }

    public long ReferenceSeq { get; set; }
}
