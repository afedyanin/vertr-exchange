using Vertr.Exchange.Common;

namespace Vertr.Exchange.Server.Extensions;

internal static class L2MarketDataExtensions
{
    public static Contracts.Level2MarketData ToDto(this L2MarketData data, DateTime currentTime)
    {
        var res = new Contracts.Level2MarketData
        {
            AskSize = data.AskSize,
            BidSize = data.BidSize,
            ReferenceSeq = data.ReferenceSeq,
            Timestamp = currentTime,
            AskVolumes = data.AskVolumes,
            BidVolumes = data.BidVolumes,
            AskOrders = data.AskOrders,
            BidOrders = data.BidOrders,
            AskPrices = data.AskPrices,
            BidPrices = data.BidPrices,
        };
        return res;
    }
}
