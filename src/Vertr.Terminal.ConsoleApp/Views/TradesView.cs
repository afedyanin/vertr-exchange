using Spectre.Console;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp.Views;

internal static class TradesView
{
    public static void Render(TradeItem[] tradeItems)
    {
        if (tradeItems == null || tradeItems.Length <= 0)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var obTable = CreateTable(tradeItems);
        AnsiConsole.Write(obTable);
    }

    private static Table CreateTable(TradeItem[] tradeItems)
    {
        var table = new Table
        {
            Title = new TableTitle("Trades"),
        };

        table.AddColumns(
            "Seq #",
            // "Symbol",
            "Timestamp",
            "Taker Action",
            "Taker",
            "Maker",
            "Taker OrderId",
            "Maker OrderId",
            // "Take OrderCompleted",
            // "Maker OrderCompleted",
            "Price",
            "Volume" //,
            //"TotalVolume"
            );

        foreach (var ti in tradeItems)
        {
            var symbol = Symbols.GetById(ti.Symbol);
            var taker = Users.GetById(ti.TakerUid);
            var maker = Users.GetById(ti.MakerUid);

            table.AddRow(
                ti.Seq.ToString(),
                //symbol!.Code,
                ti.Timestamp.ToString(ViewConsts.TimeFormat),
                ti.TakerAction.ToString(),
                taker!.Name,
                maker!.Name,
                ti.TakerOrderId.ToString(),
                ti.MakerOrderId.ToString(),
                // ti.TakeOrderCompleted.ToString(),
                // ti.MakerOrderCompleted.ToString(),
                ti.Price.ToString(ViewConsts.DecimalFormat),
                ti.Volume.ToString());
            // ti.TotalVolume.ToString());
        };

        return table;
    }
}
