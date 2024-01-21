using Vertr.Exchange.Contracts;

namespace Vertr.Terminal.Domain.PortfolioManagement;

public static class PositionTradeFactory
{
    public static PositionTrade[] Create(TradeEvent tradeEvent)
    {
        var res = new List<PositionTrade>();

        foreach (var makerTrade in tradeEvent.Trades)
        {
            res.Add(CreateTakerTrade(tradeEvent, makerTrade));
            res.Add(CreateMakerTrade(tradeEvent, makerTrade));
        }

        return [.. res];
    }

    private static PositionTrade CreateTakerTrade(TradeEvent tradeEvent, Trade makerTrade)
    {
        return new PositionTrade
        {
            Direction = tradeEvent.TakerAction == Exchange.Shared.Enums.OrderAction.ASK ?
                Exchange.Shared.Enums.PositionDirection.DIR_SHORT :
                Exchange.Shared.Enums.PositionDirection.DIR_LONG,

            OrderId = tradeEvent.TakerOrderId,
            Uid = tradeEvent.TakerUid,

            Symbol = tradeEvent.Symbol,
            Timestamp = tradeEvent.Timestamp,
            Seq = tradeEvent.Seq,
            Volume = makerTrade.Volume,
            Price = makerTrade.Price,
        };
    }

    private static PositionTrade CreateMakerTrade(TradeEvent tradeEvent, Trade makerTrade)
    {
        return new PositionTrade
        {
            // Revert direction for maker
            Direction = tradeEvent.TakerAction == Exchange.Shared.Enums.OrderAction.ASK ?
                Exchange.Shared.Enums.PositionDirection.DIR_LONG :
                Exchange.Shared.Enums.PositionDirection.DIR_SHORT,

            OrderId = makerTrade.MakerOrderId,
            Uid = makerTrade.MakerUid,

            Symbol = tradeEvent.Symbol,
            Timestamp = tradeEvent.Timestamp,
            Seq = tradeEvent.Seq,
            Volume = makerTrade.Volume,
            Price = makerTrade.Price,
        };
    }
}
