namespace Vertr.Exchange.RiskEngine.LastPriceCache;

internal readonly record struct LastPriceCacheRecord
{
    public decimal AskPrice { get; } = decimal.MaxValue;

    public decimal BidPrice { get; } = decimal.Zero;

    public LastPriceCacheRecord(decimal askPrice, decimal bidPrice)
    {
        AskPrice = askPrice;
        BidPrice = bidPrice;
    }

    public LastPriceCacheRecord Average
    {
        get
        {
            var avg = (AskPrice + BidPrice) / 2.0M;
            return new LastPriceCacheRecord(avg, avg);
        }
    }
}
