using Vertr.Exchange.Contracts;
using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.Server.Converters;

internal static class TradeEventExtensions
{
    public static TradeItem[] ToTradeItems(this TradeEvent tradeEvent)
    {
        if (tradeEvent.Trades == null || tradeEvent.Trades.Length < 1)
        {
            throw new ArgumentException($"Invalid trades count in TradeEvent. Seq={tradeEvent.Seq}");
        }

        var items = new List<TradeItem>();

        foreach (var trade in tradeEvent.Trades)
        {
            items.Add(Compose(tradeEvent, trade));
        }

        return [.. items];
    }

    private static TradeItem Compose(TradeEvent tevent, Trade trade)
    {
        return new TradeItem
        {
            Seq = tevent.Seq,
            Symbol = tevent.Symbol,
            TakeOrderCompleted = tevent.TakeOrderCompleted,
            TakerAction = tevent.TakerAction,
            TakerOrderId = tevent.TakerOrderId,
            TakerUid = tevent.TakerUid,
            Timestamp = tevent.Timestamp,
            TotalVolume = tevent.TotalVolume,
            MakerOrderCompleted = trade.MakerOrderCompleted,
            MakerOrderId = trade.MakerOrderId,
            MakerUid = trade.MakerUid,
            Price = trade.Price,
            Volume = trade.Volume,
        };
    }
}
