using Spectre.Console;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ConsoleApp.Views;

internal static class OrderBookView
{
    public static void Render(OrderBook? orderBook, string subTitle = "")
    {
        if (orderBook == null)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var symbol = StaticContext.Symbols.All.GetById(orderBook.Symbol);
        var obTable = CreateTable(symbol!, orderBook, 10, subTitle);
        AnsiConsole.Write(obTable);
    }

    private static Table CreateTable(Symbol symbol, OrderBook ob, int maxDepth, string subTitle)
    {
        var title = string.IsNullOrEmpty(subTitle) ? symbol!.Code : $"{symbol!.Code} ({subTitle})";

        var table = new Table
        {
            Title = new TableTitle(title),
        };

        table.AddColumns("Bid Orders", "Bid Size", "Bid Price", "Ask Price", "Ask Size", "Ask Orders");

        foreach (var ask in ob.Asks.OrderByDescending(ask => ask.Price).Take(maxDepth))
        {
            table.AddRow(string.Empty, string.Empty, string.Empty, ask.Price.ToString(ViewConsts.DecimalFormat), ask.Volume.ToString(), ask.Orders.ToString());
        };

        foreach (var bid in ob.Bids.OrderByDescending(bid => bid.Price).Take(maxDepth))
        {
            table.AddRow(bid.Orders.ToString(), bid.Volume.ToString(), bid.Price.ToString(ViewConsts.DecimalFormat), string.Empty, string.Empty, string.Empty);
        };

        return table;
    }
}
