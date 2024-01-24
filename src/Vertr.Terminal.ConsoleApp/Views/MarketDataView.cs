using System.Text;
using Spectre.Console;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ConsoleApp.Views;
internal static class MarketDataView
{
    public static void Render(MarketDataItemDto[] history, MarketDataItemDto current)
    {
        if (history == null || history.Length <= 0)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var table = CreateTable(history, current);

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("\n");
    }

    private static Table CreateTable(MarketDataItemDto[] history, MarketDataItemDto current)
    {
        var summary = GetSummary(current);

        var table = new Table
        {
            Title = new TableTitle(summary)
        };

        table.AddColumns(
            "Time",
            "Price",
            "Last Change",
            "Open",
            "High",
            "Low",
            "Change",
            "% Change"
            );

        foreach (var item in history)
        {
            table.AddRow(
                item.TimeStamp.ToString(ViewConsts.TimeFormat),
                item.Price.ToString(ViewConsts.DecimalFormat),
                item.LastChange.ToString(ViewConsts.DecimalFormat),
                item.DayOpen.ToString(ViewConsts.DecimalFormat),
                item.DayHigh.ToString(ViewConsts.DecimalFormat),
                item.DayLow.ToString(ViewConsts.DecimalFormat),
                item.Change.ToString(ViewConsts.DecimalFormat),
                item.PercentChange.ToString(ViewConsts.DecimalFormat)
                );
        }

        return table;
    }

    private static string GetSummary(MarketDataItemDto current)
    {
        var symbol = StaticContext.Symbols.All.GetById(current.SymbolId);

        var sb = new StringBuilder("Market data: ");
        sb.Append($"S={symbol!.Code} ");
        sb.Append($"Time={current.TimeStamp.ToString(ViewConsts.DateTimeFormat)} ");
        sb.Append($"Price={current.Price.ToString(ViewConsts.DecimalFormat)} ");
        sb.Append($"Open={current.DayOpen.ToString(ViewConsts.DecimalFormat)} ");
        sb.Append($"High={current.DayHigh.ToString(ViewConsts.DecimalFormat)} ");
        sb.Append($"Low={current.DayLow.ToString(ViewConsts.DecimalFormat)} ");
        sb.Append($"Change={current.Change.ToString(ViewConsts.DecimalFormat)} ");
        sb.Append($"% Change={current.PercentChange.ToString(ViewConsts.DecimalFormat)} ");
        sb.Append($"Last Change={current.LastChange.ToString(ViewConsts.DecimalFormat)} ");
        return sb.ToString();
    }
}
