using Google.Protobuf.WellKnownTypes;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class OrderBookExtensions
{
    public static OrderBook ToProto(this Common.Messages.OrderBook orderBook)
    {
        var res = new OrderBook
        {
            Symbol = orderBook.Symbol,
            Timestamp = orderBook.Timestamp.ToTimestamp(),
            Seq = orderBook.Seq,
        };

        res.Asks.AddRange(orderBook.Asks.ToProto());
        res.Bids.AddRange(orderBook.Bids.ToProto());

        return res;
    }

    private static IEnumerable<OrderBookRecord> ToProto(this IEnumerable<Common.Messages.OrderBookRecord> orderBookRecords)
        => orderBookRecords.Select(ToProto);

    private static OrderBookRecord ToProto(this Common.Messages.OrderBookRecord orderBookRecord)
        => new OrderBookRecord
        {
            Orders = orderBookRecord.Orders,
            Price = orderBookRecord.Price,
            Volume = orderBookRecord.Volume,
        };
}
