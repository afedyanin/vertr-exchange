using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Commands;
internal class MarketDataSnapshotCommand : OrderBookCommand
{
    public MarketDataSnapshotCommand(OrderBook orderBook, OrderCommand cmd) : base(orderBook, cmd)
    {
    }

    public override CommandResultCode Execute()
    {
        var size = (int)OrderCommand.Size;
        OrderCommand.MarketData = GetL2MarketDataSnapshot(size >= 0 ? size : int.MaxValue);
        return CommandResultCode.SUCCESS;
    }

    public L2MarketData GetL2MarketDataSnapshot(int size)
    {
        var asksSize = GetTotalAskBuckets(size);
        var bidsSize = GetTotalBidBuckets(size);
        var data = new L2MarketData(asksSize, bidsSize);
        FillAsks(asksSize, data);
        FillBids(bidsSize, data);
        return data;
    }

    private int GetTotalAskBuckets(int limit)
        => Math.Min(limit, _askBuckets.Count);

    private int GetTotalBidBuckets(int limit)
        => Math.Min(limit, _bidBuckets.Count);

    private void FillAsks(int size, L2MarketData data)
    {
        if (size == 0)
        {
            data.AskSize = 0;
            return;
        }

        var i = 0;

        foreach (var bucket in _askBuckets.Values)
        {
            data.AskPrices[i] = bucket.Price;
            data.AskVolumes[i] = bucket.TotalVolume;
            data.AskOrders[i] = bucket.OrdersCount;

            if (++i == size)
            {
                break;
            }
        }

        data.AskSize = i;
    }

    private void FillBids(int size, L2MarketData data)
    {
        if (size == 0)
        {
            data.BidSize = 0;
            return;
        }

        var i = 0;

        foreach (var bucket in _bidBuckets.Values)
        {
            data.BidPrices[i] = bucket.Price;
            data.BidVolumes[i] = bucket.TotalVolume;
            data.BidOrders[i] = bucket.OrdersCount;
            if (++i == size)
            {
                break;
            }
        }
        data.BidSize = i;
    }

}
