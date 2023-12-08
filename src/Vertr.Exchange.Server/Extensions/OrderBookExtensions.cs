using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Server.Extensions;

internal static class OrderBookExtensions
{
    public static OrderBook ToDto(this Common.Messages.OrderBook orderBook)
    {
        var res = new OrderBook
        {
            Symbol = orderBook.Symbol,
            Timestamp = orderBook.Timestamp,
            Seq = orderBook.Seq,
            Asks = orderBook.Asks.ToDto().ToArray(),
            Bids = orderBook.Bids.ToDto().ToArray(),
        };

        return res;
    }

    private static IEnumerable<OrderBookRecord> ToDto(this IEnumerable<Common.Messages.OrderBookRecord> orderBookRecords)
        => orderBookRecords.Select(ToDto);

    private static OrderBookRecord ToDto(this Common.Messages.OrderBookRecord orderBookRecord)
        => new OrderBookRecord
        {
            Orders = orderBookRecord.Orders,
            Price = orderBookRecord.Price,
            Volume = orderBookRecord.Volume,
        };
}
