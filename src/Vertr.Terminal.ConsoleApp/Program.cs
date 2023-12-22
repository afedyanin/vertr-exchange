using Refit;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.Views;

namespace Vertr.Terminal.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        await RunStrategy();
    }

    public static async Task RunStrategy()
    {
        var api = RestService.For<ITerminalApiClient>("http://localhost:5010");
        var commands = new ApiCommands(api);

        await commands.Reset();

        await commands.AddSymbols(StaticContext.Symbols.All);

        await commands.AddUsers([
            StaticContext.UserAccounts.BobAccount,
            StaticContext.UserAccounts.AliceAccount,
        ]);

        var bobTrading = Task.Run(async () =>
        {
            await commands.RandomWalk(StaticContext.Users.Bob, StaticContext.Symbols.MSFT, 100, 100);
        });

        var aliceTrading = Task.Run(async () =>
        {
            await commands.RandomWalk(StaticContext.Users.Alice, StaticContext.Symbols.MSFT, 100, 100);
        });

        await Task.WhenAll(aliceTrading, bobTrading);

        /*

        var ob = await api.GetOrderBook(Symbols.MSFT.Id);
        OrderBookView.Render(ob, "Random walk");

        var trades = await api.GetTrades();
        TradesView.Render(trades);

        var req = new UserRequest()
        {
            UserId = Users.Bob.Id,
        };

        var report = await commands.GetSingleUserReport(req);
        SingleUserReportView.Render(report);

        req = new UserRequest()
        {
            UserId = Users.Alice.Id,
        };

        report = await commands.GetSingleUserReport(req);
        SingleUserReportView.Render(report);
        */

        var orders = await api.GetOrders();
        OrdersView.Render(orders);
    }
}
