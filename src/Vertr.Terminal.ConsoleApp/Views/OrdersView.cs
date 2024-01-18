using Spectre.Console;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.ApiClient.Extensions;

namespace Vertr.Terminal.ConsoleApp.Views;

internal static class OrdersView
{
    public static void Render(OrderDto[] orders)
    {
        if (orders == null || orders.Length <= 0)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var tables = CreateTables(orders);

        foreach (var table in tables)
        {
            AnsiConsole.Write(table);
            AnsiConsole.WriteLine("\n");
        }
    }

    private static Table[] CreateTables(OrderDto[] orders)
    {
        var tables = new Table[orders.Length];

        for (int i = 0; i < orders.Length; i++)
        {
            tables[i] = CreateTable(orders[i]);
        }

        return tables;
    }

    private static Table CreateTable(OrderDto order)
    {
        var symbol = StaticContext.Symbols.All.GetById(order.Symbol);
        var user = StaticContext.Users.All.GetById(order.UserId);

        var events = order.OrderEvents.ToArray();
        var isCompleted = events.Where(e => e.OrderCompleted).Any();

        var table = new Table
        {
            Title = new TableTitle($"Order: U={user!.Name} S={symbol!.Code} T={order.OrderType} A={order.Action} Id={order.OrderId} Q={order.Size} P={order.Price.ToString(ViewConsts.DecimalFormat)} IsCompleted={isCompleted}")
        };

        table.AddColumns(
            "Seq",
            "TimeStamp",
            "Action",
            "CommandResultCode",
            "EventSource",
            "Price",
            "Volume",
            "OrderCompleted"
            );

        if (events.Length <= 0)
        {
            return table;
        }

        foreach (var evt in events)
        {
            table.AddRow(
                evt.Seq.ToString(),
                evt.TimeStamp.ToString(ViewConsts.TimeFormat),
                evt.Action.HasValue ? evt.Action.Value.ToString() : string.Empty,
                evt.CommandResultCode.HasValue ? evt.CommandResultCode.Value.ToString() : string.Empty,
                evt.EventSource.ToString(),
                evt.Price.HasValue ? evt.Price.Value.ToString(ViewConsts.DecimalFormat) : string.Empty,
                evt.Volume.HasValue ? evt.Volume.Value.ToString() : string.Empty,
                evt.OrderCompleted.ToString()
                );
        }

        return table;
    }
}
