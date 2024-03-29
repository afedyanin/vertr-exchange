using Vertr.Exchange.Contracts;
using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Adapters.SignalR.Extensions;

internal static class EventExtensions
{
    public static IEnumerable<ExchangeEvent> ToDto(this IEngineEvent rootEvent)
    {
        var res = new List<ExchangeEvent>();
        var current = rootEvent;

        while (current != null)
        {
            res.Add(current.ToDtoSingle());
            current = current.NextEvent;
        }

        return res;
    }

    public static RejectEvent ToDto(this Application.Messages.RejectEvent rejectEvent)
        => new RejectEvent
        {
            OrderId = rejectEvent.OrderId,
            Price = rejectEvent.Price,
            RejectedVolume = rejectEvent.RejectedVolume,
            Symbol = rejectEvent.Symbol,
            Timestamp = rejectEvent.Timestamp,
            Uid = rejectEvent.Uid,
            Seq = rejectEvent.Seq,
        };

    public static ReduceEvent ToDto(this Application.Messages.ReduceEvent reduceEvent)
        => new ReduceEvent
        {
            Uid = reduceEvent.Uid,
            Price = reduceEvent.Price,
            OrderCompleted = reduceEvent.OrderCompleted,
            Symbol = reduceEvent.Symbol,
            OrderId = reduceEvent.OrderId,
            ReducedVolume = reduceEvent.ReducedVolume,
            Timestamp = reduceEvent.Timestamp,
            Seq = reduceEvent.Seq,
        };

    public static TradeEvent ToDto(this Application.Messages.TradeEvent tradeEvent)
    {
        var res = new TradeEvent
        {
            Symbol = tradeEvent.Symbol,
            TakeOrderCompleted = tradeEvent.TakeOrderCompleted,
            TakerAction = (Contracts.Enums.OrderAction)tradeEvent.TakerAction,
            TakerUid = tradeEvent.TakerUid,
            TakerOrderId = tradeEvent.TakerOrderId,
            Timestamp = tradeEvent.Timestamp,
            TotalVolume = tradeEvent.TotalVolume,
            Seq = tradeEvent.Seq,
            Trades = tradeEvent.Trades.ToDto().ToArray(),
        };

        return res;
    }

    private static ExchangeEvent ToDtoSingle(this IEngineEvent evt)
        => new ExchangeEvent()
        {
            ActiveOrderCompleted = evt.ActiveOrderCompleted,
            MatchedOrderCompleted = evt.MatchedOrderCompleted,
            MatchedOrderId = evt.MatchedOrderId,
            MatchedOrderUid = evt.MatchedOrderUid,
            Price = evt.Price,
            Size = evt.Size,
            BinaryData = evt.BinaryData,
            EventType = (Contracts.Enums.EngineEventType)evt.EventType,
        };

    private static IEnumerable<Trade> ToDto(this IEnumerable<Application.Messages.Trade> trades)
        => trades.Select(ToDto);

    private static Trade ToDto(Application.Messages.Trade trade)
        => new Trade
        {
            MakerOrderCompleted = trade.MakerOrderCompleted,
            MakerOrderId = trade.MakerOrderId,
            MakerUid = trade.MakerUid,
            Price = trade.Price,
            Volume = trade.Volume,
        };
}
