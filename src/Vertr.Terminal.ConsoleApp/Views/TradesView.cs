using Spectre.Console;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp.Views;

internal static class TradesView
{
    public static void Render(TradeEvent[] tradeEvents)
    {
        if (tradeEvents == null || tradeEvents.Length <= 0)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var obTable = CreateTable(tradeEvents);
        AnsiConsole.Write(obTable);
    }

    private static Table CreateTable(TradeEvent[] tradeEvents)
    {
        var table = new Table
        {
            Title = new TableTitle("Trades"),
        };

        table.AddColumns(
            "Symbol",
            "Seq #",
            "Timestamp",
            "Tkr Vol",
            "Vol",
            "Price",
            "Tkr Action",
            "Tkr OrdId",
            "Mkr OrdId"
            );

        foreach (var tEvent in tradeEvents)
        {
            var symbol = Symbols.GetById(tEvent.Symbol);

            for (int i = 0; i < tEvent.Trades.Length; i++)
            {
                var trade = tEvent.Trades[i];
                var maker = Users.GetById(trade.MakerUid);

                table.AddRow(
                    symbol!.Code,
                    i == 0 ? tEvent.Seq.ToString() : ViewConsts.Empty,
                    tEvent.Timestamp.ToString(ViewConsts.TimeFormat),
                    i == 0 ? tEvent.TotalVolume.ToString() : ViewConsts.Empty,
                    trade.Volume.ToString(),
                    trade.Price.ToString(ViewConsts.DecimalFormat),
                    i == 0 ? tEvent.TakerAction.ToString() : ViewConsts.Empty,
                    i == 0 ? tEvent.TakerOrderId.ToString() : ViewConsts.Empty,
                    trade.MakerOrderId.ToString()
                    );
            }
        };

        return table;
    }
}
