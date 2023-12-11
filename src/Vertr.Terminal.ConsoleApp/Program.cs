using Refit;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.StaticData;
using Vertr.Terminal.ConsoleApp.Views;

namespace Vertr.Terminal.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        var bobTrading = Task.Run(async () =>
        {
            await Work(Users.Bob.Name, 100);
        });

        var aliceTrading = Task.Run(async () =>
        {
            await Work(Users.Alice.Name, 100);
        });

        await Task.WhenAll(aliceTrading, bobTrading);

        Console.WriteLine($"Execution completed.");
    }

    private static async Task Work(string name, int count)
    {
        Console.WriteLine($"Job {name} started.");
        var client = RestService.For<ITerminalApiClient>("http://localhost:5010");
        var commands = new Commands(client);

        for (var i = 0; i < count; i++)
        {
            var res = await commands.Nop();
            await Task.Delay(Random.Shared.Next(0, 10));
            Console.WriteLine($"Job {name} iteration #{i} completed. Seq={res!.Seq}  OrderId={res.OrderId}");
        }

        Console.WriteLine($"Job {name} completed.");
    }

    public static async Task RunStrategy()
    {
        var api = RestService.For<ITerminalApiClient>("http://localhost:5010");
        var commands = new Commands(api);

        var res = await commands.Reset();
        //Console.WriteLine(res);

        res = await commands.AddSymbols();
        //Console.WriteLine(res);

        res = await commands.AddUsers();
        //Console.WriteLine(res);

        var bobTrading = Task.Run(() =>
        {
            TradingStrategy.RandomWalkTrading(Users.Bob, Symbols.MSFT, 100m, 0.01m, 100).GetAwaiter().GetResult();
        });

        var aliceTrading = Task.Run(() =>
        {
            TradingStrategy.RandomWalkTrading(Users.Alice, Symbols.MSFT, 100m, 0.01m, 100).GetAwaiter().GetResult();
        });

        await Task.WhenAll(aliceTrading, bobTrading);

        //var ob = await api.GetOrderBook(Symbols.MSFT.Id);
        //OrderBookView.Render(ob, "Random walk");

        // var trades = await api.GetTrades();
        // TradesView.Render(trades);

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
    }
}
