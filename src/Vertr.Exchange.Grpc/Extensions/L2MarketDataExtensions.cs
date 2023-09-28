using Google.Protobuf.WellKnownTypes;
using Vertr.Exchange.Common;

namespace Vertr.Exchange.Grpc.Extensions;

internal static class L2MarketDataExtensions
{
    public static Level2MarketData ToGrpc(this L2MarketData data)
    {
        var res = new Level2MarketData
        {
            AskSize = data.AskSize,
            BidSize = data.BidSize,
            Timestamp = data.Timestamp.ToTimestamp(),
            ReferenceSeq = data.ReferenceSeq,
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
