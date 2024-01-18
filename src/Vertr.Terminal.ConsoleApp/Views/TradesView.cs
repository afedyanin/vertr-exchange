using Spectre.Console;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ConsoleApp.Views;

internal static class TradesView
{
    public static void Render(TradeEvent[]? tradeEvents)
    {
        if (tradeEvents == null || tradeEvents.Length <= 0)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var obTable = CreateTable(tradeEvents);
        AnsiConsole.Write(obTable);
        AnsiConsole.WriteLine("\n");
    }

    private static Table CreateTable(TradeEvent[] tradeEvents)
    {
        var totalVol = CalculateTotalVol(tradeEvents);

        var table = new Table
        {
            Title = new TableTitle($"Trades Total Vol={totalVol.ToString(ViewConsts.DecimalFormat)}"),
        };

        table.AddColumns(
            "Symbol",
            "Seq #",
            "Timestamp",
            "Tkr User",
            "Tkr Size",
            "Total Size",
            "Price",
            "Vol",
            "Tkr Action",
            "Tkr OrdId",
            "Mkr OrdId"
            );

        foreach (var tEvent in tradeEvents)
        {
            var symbol = StaticContext.Symbols.All.GetById(tEvent.Symbol);
            var usr = StaticContext.Users.All.GetById(tEvent.TakerUid);

            for (int i = 0; i < tEvent.Trades.Length; i++)
            {
                var trade = tEvent.Trades[i];

                table.AddRow(
                    symbol!.Code,
                    i == 0 ? tEvent.Seq.ToString() : ViewConsts.Empty,
                    tEvent.Timestamp.ToString(ViewConsts.TimeFormat),
                    usr!.Name,
                    i == 0 ? tEvent.TotalVolume.ToString() : ViewConsts.Empty,
                    trade.Volume.ToString(),
                    trade.Price.ToString(ViewConsts.DecimalFormat),
                    (trade.Volume * trade.Price).ToString(ViewConsts.DecimalFormat),
                    i == 0 ? tEvent.TakerAction.ToString() : ViewConsts.Empty,
                    i == 0 ? tEvent.TakerOrderId.ToString() : ViewConsts.Empty,
                    trade.MakerOrderId.ToString()
                    );
            }
        };

        return table;
    }

    private static decimal CalculateTotalVol(TradeEvent[]? tradeEvents)
    {
        var totalVol = tradeEvents == null ?
            decimal.Zero :
            tradeEvents.Sum(te => te.Trades.Sum(t => t.Price * t.Volume));

        return totalVol;
    }

}
