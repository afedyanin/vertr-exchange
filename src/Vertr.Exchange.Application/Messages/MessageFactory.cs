using Vertr.Exchange.Application.Messages;
using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.Common.Messages;

public static class MessageFactory
{
    public static ApiCommandResult CreateApiCommandResult(OrderCommand cmd, long seq)
        => new ApiCommandResult
        {
            OrderId = cmd.OrderId,
            Uid = cmd.Uid,
            ResultCode = cmd.ResultCode,
            Seq = seq,
            Timestamp = cmd.Timestamp,
            BinaryCommandType = cmd.BinaryCommandType,
            BinaryData = GetBynaryResult(cmd),
        };

    public static RejectEvent CreateRejectEvent(OrderCommand cmd, IEngineEvent evt, long seq)
        => new RejectEvent
        {
            OrderId = cmd.OrderId,
            Price = evt.Price,
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp,
            Uid = cmd.Uid,
            RejectedVolume = evt.Size,
            Seq = seq,
        };

    public static ReduceEvent CreateReduceEvent(OrderCommand cmd, IEngineEvent evt, long seq)
        => new ReduceEvent
        {
            OrderId = cmd.OrderId,
            Price = evt.Price,
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp,
            Uid = cmd.Uid,
            OrderCompleted = evt.ActiveOrderCompleted,
            ReducedVolume = evt.Size,
            Seq = seq,
        };

    public static TradeEvent CreateTradeEvent(OrderCommand cmd, IEnumerable<IEngineEvent> tradeEngineEvents, long seq)
    {
        var res = new TradeEvent
        {
            TakeOrderCompleted = tradeEngineEvents.Any(t => t.ActiveOrderCompleted),
            TakerAction = cmd.Action!.Value,
            TakerOrderId = cmd.OrderId,
            TakerUid = cmd.Uid,
            TotalVolume = tradeEngineEvents.Sum(t => t.Size),
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp,
            Trades = CreateTrades(tradeEngineEvents),
            Seq = seq,
        };

        return res;
    }

    public static OrderBook CreateOrderBook(OrderCommand cmd, L2MarketData marketData, long seq)
    {
        var res = new OrderBook
        {
            Timestamp = cmd.Timestamp,
            Symbol = cmd.Symbol,
            Asks = CreateAsks(marketData),
            Bids = CreateBids(marketData),
            Seq = seq,
        };

        return res;
    }

    private static byte[] GetBynaryResult(OrderCommand cmd)
    {
        var evt = cmd.EngineEvent;

        if (evt == null || evt.EventType != Shared.Enums.EngineEventType.BINARY_EVENT)
        {
            return [];
        }

        return evt.BinaryData;
    }

    private static IEnumerable<Trade> CreateTrades(IEnumerable<IEngineEvent> trades)
        => trades.Select(CreateTrade);

    private static Trade CreateTrade(IEngineEvent tradeEvt)
        => new Trade
        {
            MakerOrderCompleted = tradeEvt.MatchedOrderCompleted,
            MakerOrderId = tradeEvt.MatchedOrderId,
            MakerUid = tradeEvt.MatchedOrderUid,
            Price = tradeEvt.Price,
            Volume = tradeEvt.Size,
        };

    private static OrderBookRecord[] CreateAsks(L2MarketData mdata)
    {
        var res = new OrderBookRecord[mdata.AskSize];

        for (var i = 0; i < mdata.AskSize; i++)
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

    private static OrderBookRecord[] CreateBids(L2MarketData mdata)
    {
        var res = new OrderBookRecord[mdata.BidSize];

        for (var i = 0; i < mdata.BidSize; i++)
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
