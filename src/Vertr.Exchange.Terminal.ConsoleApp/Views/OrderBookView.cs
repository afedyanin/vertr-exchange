using Spectre.Console;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Terminal.ConsoleApp.StaticData;

namespace Vertr.Exchange.Terminal.ConsoleApp.Views;

internal static class OrderBookView
{
    private const string decimalFormat = "####.####";

    public static void Render(OrderBook? orderBook)
    {
        if (orderBook == null)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var symbol = Symbols.GetById(orderBook.Symbol);
        var obTable = CreateTable(symbol!, orderBook);
        AnsiConsole.Write(obTable);
    }

    private static Table CreateTable(Symbol symbol, OrderBook ob, int maxDepth = 10)
    {
        var table = new Table
        {
            Title = new TableTitle(symbol!.Code),
        };

        table.AddColumns("Bid Orders", "Bid Size", "Bid Price", "Ask Price", "Ask Size", "Ask Orders");

        foreach (var ask in ob.Asks.OrderByDescending(ask => ask.Price).Take(maxDepth))
        {
            table.AddRow(string.Empty, string.Empty, string.Empty, ask.Price.ToString(decimalFormat), ask.Volume.ToString(), ask.Orders.ToString());
        };

        foreach (var bid in ob.Bids.OrderByDescending(bid => bid.Price).Take(maxDepth))
        {
            table.AddRow(bid.Orders.ToString(), bid.Volume.ToString(), bid.Price.ToString(decimalFormat), string.Empty, string.Empty, string.Empty);
        };

        return table;
    }
}
