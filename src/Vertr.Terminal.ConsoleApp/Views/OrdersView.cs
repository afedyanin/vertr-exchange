using Spectre.Console;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp.Views;

internal static class OrdersView
{
    public static void Render(Order[] orders)
    {
        if (orders == null || orders.Length <= 0)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var obTable = CreateTable(orders);
        AnsiConsole.Write(obTable);
    }

    private static Table CreateTable(Order[] orders)
    {
        var table = new Table
        {
            Title = new TableTitle("Orders"),
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

        foreach (var order in orders)
        {
            var symbol = Symbols.GetById(order.Symbol);
            var user = Users.GetById(order.UserId);
            var events = order.GetEvents();

            for (int i = 0; i < events.Length; i++)
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
