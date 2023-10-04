using Google.Protobuf.WellKnownTypes;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Api.Factories;

internal static class MessageFactory
{
    public static ApiCommandResult CreateApiCommandResult(OrderCommand cmd, long seq)
        => new ApiCommandResult
        {
            OrderId = cmd.OrderId,
            Uid = cmd.Uid,
            ResultCode = (ResultCode)cmd.ResultCode,
            Seq = seq,
            Timestamp = cmd.Timestamp.ToTimestamp(),
        };

    public static RejectEvent CreateRejectEvent(OrderCommand cmd, IEngineEvent evt)
        => new RejectEvent
        {
            OrderId = cmd.OrderId,
            Price = evt.Price,
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp.ToTimestamp(),
            Uid = cmd.Uid,
            RejectedVolume = evt.Size
        };

    public static ReduceEvent CreateReduceEvent(OrderCommand cmd, IEngineEvent evt)
        => new ReduceEvent
        {
            OrderId = cmd.OrderId,
            Price = evt.Price,
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp.ToTimestamp(),
            Uid = cmd.Uid,
            OrderCompleted = evt.ActiveOrderCompleted,
            ReducedVolume = evt.Size,
        };

    public static TradeEvent CreateTradeEvent(OrderCommand cmd, IEnumerable<IEngineEvent> tradeEngineEvents)
    {
        var res = new TradeEvent
        {
            TakeOrderCompleted = tradeEngineEvents.Any(t => t.ActiveOrderCompleted),
            TakerAction = (OrderAction)cmd.Action!,
            TakerOrderId = cmd.OrderId,
            TakerUid = cmd.Uid,
            TotalVolume = tradeEngineEvents.Sum(t => t.Size),
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp.ToTimestamp(),
        };

        res.Trades.AddRange(CreateTrades(tradeEngineEvents));

        return res;
    }

    private static IEnumerable<Trade> CreateTrades(IEnumerable<IEngineEvent> trades)
        => trades.Select(t => CreateTrade(t));

    private static Trade CreateTrade(IEngineEvent tradeEvt)
        => new Trade
        {
            MakerOrderCompleted = tradeEvt.MatchedOrderCompleted,
            MakerOrderId = tradeEvt.MatchedOrderId,
            MakerUid = tradeEvt.MatchedOrderUid,
            Price = tradeEvt.Price,
            Volume = tradeEvt.Size,
        };

    public static OrderBook CreateOrderBook(OrderCommand cmd, L2MarketData marketData)
    {
        var res = new OrderBook
        {
            Timestamp = cmd.Timestamp.ToTimestamp(),
            Symbol = cmd.Symbol,
        };

        res.Asks.AddRange(CreateAsks(marketData));
        res.Bids.AddRange(CreateBids(marketData));

        return res;
    }

    private static IEnumerable<OrderBookRecord> CreateAsks(L2MarketData mdata)
    {
        var res = new OrderBookRecord[mdata.AskSize];

        for (int i = 0; i < mdata.AskSize; i++)
        {
            res[i] = new OrderBookRecord
            {
                Orders = mdata.AskOrders[i],
                Price = mdata.AskPrices[i],
                Volume = mdata.AskVolumes[i],
            };
        }

        return res;
    }

    private static IEnumerable<OrderBookRecord> CreateBids(L2MarketData mdata)
    {
        var res = new OrderBookRecord[mdata.BidSize];

        for (int i = 0; i < mdata.BidSize; i++)
        {
            res[i] = new OrderBookRecord
            {
                Orders = mdata.BidOrders[i],
                Price = mdata.BidPrices[i],
                Volume = mdata.BidVolumes[i],
            };
        }

        return res;
    }
}
