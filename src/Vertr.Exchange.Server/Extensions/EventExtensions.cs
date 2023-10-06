using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class EventExtensions
{
    public static IEnumerable<ExchangeEvent> ToProto(this IEngineEvent rootEvent)
    {
        var res = new List<ExchangeEvent>();
        var current = rootEvent;

        while (current != null)
        {
            res.Add(current.ToProtoSingle());
            current = current.NextEvent;
        }

        return res;
    }

    public static RejectEvent ToProto(this Common.Messages.RejectEvent rejectEvent)
        => new RejectEvent
        {
            OrderId = rejectEvent.OrderId,
            Price = rejectEvent.Price,
            RejectedVolume = rejectEvent.RejectedVolume,
            Symbol = rejectEvent.Symbol,
            Timestamp = rejectEvent.Timestamp.ToTimestamp(),
            Uid = rejectEvent.Uid,
            Seq = rejectEvent.Seq,
        };

    public static ReduceEvent ToProto(this Common.Messages.ReduceEvent reduceEvent)
        => new ReduceEvent
        {
            Uid = reduceEvent.Uid,
            Price = reduceEvent.Price,
            OrderCompleted = reduceEvent.OrderCompleted,
            Symbol = reduceEvent.Symbol,
            OrderId = reduceEvent.OrderId,
            ReducedVolume = reduceEvent.ReducedVolume,
            Timestamp = reduceEvent.Timestamp.ToTimestamp(),
            Seq = reduceEvent.Seq,
        };

    public static TradeEvent ToProto(this Common.Messages.TradeEvent tradeEvent)
    {
        var res = new TradeEvent
        {
            Symbol = tradeEvent.Symbol,
            TakeOrderCompleted = tradeEvent.TakeOrderCompleted,
            TakerAction = tradeEvent.TakerAction.ToProto(),
            TakerUid = tradeEvent.TakerUid,
            TakerOrderId = tradeEvent.TakerOrderId,
            Timestamp = tradeEvent.Timestamp.ToTimestamp(),
            TotalVolume = tradeEvent.TotalVolume,
            Seq = tradeEvent.Seq,
        };

        res.Trades.AddRange(tradeEvent.Trades.ToProto());

        return res;
    }

    private static ExchangeEvent ToProtoSingle(this IEngineEvent evt)
        => new ExchangeEvent()
        {
            ActiveOrderCompleted = evt.ActiveOrderCompleted,
            MatchedOrderCompleted = evt.MatchedOrderCompleted,
            MatchedOrderId = evt.MatchedOrderId,
            MatchedOrderUid = evt.MatchedOrderUid,
            Price = evt.Price,
            Size = evt.Size,
            BinaryData = ByteString.CopyFrom(evt.BinaryData),
            EventType = evt.EventType.ToProto(),
        };

    private static IEnumerable<Trade> ToProto(this IEnumerable<Common.Messages.Trade> trades)
        => trades.Select(ToProto);

    private static Trade ToProto(Common.Messages.Trade trade)
        => new Trade
        {
            MakerOrderCompleted = trade.MakerOrderCompleted,
            MakerOrderId = trade.MakerOrderId,
            MakerUid = trade.MakerUid,
            Price = trade.Price,
            Volume = trade.Volume,
        };
}
