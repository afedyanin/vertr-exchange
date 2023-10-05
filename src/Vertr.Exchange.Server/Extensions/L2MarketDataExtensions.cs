using Google.Protobuf.WellKnownTypes;
using Vertr.Exchange.Common;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class L2MarketDataExtensions
{
    public static Level2MarketData ToProto(this L2MarketData data, DateTime currentTime)
    {
        var res = new Level2MarketData
        {
            AskSize = data.AskSize,
            BidSize = data.BidSize,
            ReferenceSeq = data.ReferenceSeq,
            Timestamp = currentTime.ToTimestamp(),
        };

        res.AskVolumes.AddRange(data.AskVolumes);
        res.BidVolumes.AddRange(data.BidVolumes);
        res.AskOrders.AddRange(data.AskOrders);
        res.BidOrders.AddRange(data.BidOrders);
        res.AskPrices.AddRange(data.AskPrices.ToDecimalValues());
        res.BidPrices.AddRange(data.BidPrices.ToDecimalValues());

        return res;
    }

    private static IEnumerable<DecimalValue> ToDecimalValues(this decimal[] items)
        => items.Select(x => new DecimalValue(x));
}
