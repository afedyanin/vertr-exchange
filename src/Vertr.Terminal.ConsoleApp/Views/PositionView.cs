using System.Text;
using Spectre.Console;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ConsoleApp.Views;
internal static class PositionView
{
    public static void Render(PositionDto position)
    {
        var table = CreateTable(position);
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("\n");
    }

    private static Table CreateTable(PositionDto position)
    {
        var summary = GetPositionSummary(position);

        var table = new Table
        {
            Title = new TableTitle(summary)
        };

        table.AddColumns(
            "Seq",
            "TimeStamp",
            "Order Id",
            "Direction",
            "Price",
            "Size",
            "Buy Value",
            "Sell Value"
            );

        var trades = position.Trades;

        if (trades.Length <= 0)
        {
            return table;
        }

        foreach (var trade in trades)
        {
            table.AddRow(
                trade.Seq.ToString(),
                trade.Timestamp.ToString(ViewConsts.TimeFormat),
                trade.OrderId.ToString(),
                trade.Direction.ToString(),
                trade.Price.ToString(ViewConsts.DecimalFormat),
                trade.Volume.ToString(),
                trade.Direction == Exchange.Shared.Enums.PositionDirection.DIR_LONG ?
                    (trade.Price * trade.Volume).ToString(ViewConsts.DecimalFormat) :
                    string.Empty,
                trade.Direction == Exchange.Shared.Enums.PositionDirection.DIR_SHORT ?
                    (trade.Price * trade.Volume).ToString(ViewConsts.DecimalFormat) :
                    string.Empty
            );
        }

        return table;
    }


    private static string GetPositionSummary(PositionDto position)
    {
        var symbol = StaticContext.Symbols.All.GetById(position.Symbol);
        var user = StaticContext.Users.All.GetById(position.Uid);

        var sb = new StringBuilder("Position trades: ");
        sb.Append($"U={user!.Name} ");
        sb.Append($"S={symbol!.Code} ");
        return sb.ToString();
    }
}
