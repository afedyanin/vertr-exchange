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
            "Sell Value",
            "Open Price Sum",
            "Open Size",
            "Pnl Direction",
            "Fixed PnL",
            "PnL"
            );

        var trades = position.Trades;

        if (trades.Length <= 0)
        {
            return table;
        }

        for (int i = 0; i < trades.Length; i++)
        {
            var trade = trades[i];
            var pnl = position.PnlHistory[i];

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
                    string.Empty,
                pnl.OpenPriceSum.ToString(ViewConsts.DecimalFormat),
                pnl.OpenVolume.ToString(),
                pnl.Direction.ToString(),
                pnl.FixedPnL.ToString(ViewConsts.DecimalFormat),
                pnl.PnL.ToString(ViewConsts.DecimalFormat)
            );
        }

        return table;
    }


    private static string GetPositionSummary(PositionDto position)
    {
        var symbol = StaticContext.Symbols.All.GetById(position.Symbol);
        var user = StaticContext.Users.All.GetById(position.Uid);
        var pnl = position.Pnl!;

        var sb = new StringBuilder("Position trades: ");
        sb.Append($"U={user!.Name} ");
        sb.Append($"S={symbol!.Code} ");
        sb.Append($"D={pnl.Direction} ");
        sb.Append($"Fixed PnL={pnl.FixedPnL.ToString(ViewConsts.DecimalFormat)} ");
        sb.Append($"Open Size={pnl.OpenVolume.ToString()} ");
        sb.Append($"Open Price Sum={pnl.OpenPriceSum.ToString(ViewConsts.DecimalFormat)} ");
        return sb.ToString();
    }
}
