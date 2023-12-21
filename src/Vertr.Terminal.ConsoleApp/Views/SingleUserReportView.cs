using Spectre.Console;
using Vertr.Exchange.Shared.Reports;
using Vertr.Exchange.Shared.Reports.Dtos;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp.Views;
internal static class SingleUserReportView
{
    public static void Render(SingleUserReportResult? report)
    {
        if (report == null)
        {
            Console.WriteLine("<No data>");
            return;
        }

        var user = Users.GetById(report.Uid);

        Console.WriteLine($"Report execution status={report.ExecutionStatus}");
        Console.WriteLine($"User: Id={report.Uid} Name={user!.Name}  Status={report.UserStatus}");

        var accountsTable = CreateAccountsTable(report.Accounts);
        AnsiConsole.Write(accountsTable);

        var ordersTable = CreateOrdersTable(report.Orders);
        AnsiConsole.Write(ordersTable);

        var positionsTable = CreatePositionsTable(report.Positions);
        AnsiConsole.Write(positionsTable);
    }

    private static Table CreateAccountsTable(IDictionary<int, decimal> accounts)
    {
        var table = new Table
        {
            Title = new TableTitle("Accounts"),
        };

        table.AddColumns("Currency", "Amount");

        foreach (var acc in accounts)
        {
            var currency = Currencies.GetById(acc.Key);
            table.AddRow(currency!.Code, acc.Value.ToString(ViewConsts.DecimalFormat));
        };

        return table;
    }

    private static Table CreateOrdersTable(IDictionary<int, OrderDto[]> orders)
    {
        var table = new Table
        {
            Title = new TableTitle("Orders"),
        };

        table.AddColumns(
            "Symbol",
            "OrderId",
            "Timestamp",
            "Action",
            "Size",
            "Filled",
            "Remaining",
            "Price"
            );

        foreach (var kvp in orders)
        {
            var symbol = Symbols.GetById(kvp.Key);

            foreach (var order in kvp.Value.OrderBy(v => v.Action).ThenBy(v => v.Price))
            {
                table.AddRow(
                    symbol!.Code,
                    order.OrderId.ToString(),
                    order.Timestamp.ToString(ViewConsts.TimeFormat),
                    order.Action.ToString(),
                    order.Size.ToString(),
                    order.Filled.ToString(),
                    order.Remaining.ToString(),
                    order.Price.ToString(ViewConsts.DecimalFormat)
                    );
            }
        };

        return table;
    }
    private static Table CreatePositionsTable(IDictionary<int, PositionDto> positions)
    {
        var table = new Table
        {
            Title = new TableTitle("Positions"),
        };

        table.AddColumns(
            "Symbol",
            "Direction",
            "RealizedPnL",
            "OpenVolume"
            );

        foreach (var kvp in positions)
        {
            var symbol = Symbols.GetById(kvp.Key);
            var pos = kvp.Value;

            table.AddRow(
                symbol!.Code,
                pos.Direction.ToString(),
                pos.RealizedPnL.ToString(ViewConsts.DecimalFormat),
                pos.OpenVolume.ToString(ViewConsts.DecimalFormat)
                );
        };

        return table;
    }
}
