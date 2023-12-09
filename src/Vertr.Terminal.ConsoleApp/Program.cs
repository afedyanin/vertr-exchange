using Refit;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.StaticData;
using Vertr.Terminal.ConsoleApp.Views;
// using Vertr.Terminal.ConsoleApp.Views;

namespace Vertr.Terminal.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        var api = RestService.For<ITerminalApiClient>("http://localhost:5010");
        var commands = new Commands(api);

        var res = await commands.Reset();
        //Console.WriteLine(res);

        res = await commands.AddSymbols();
        //Console.WriteLine(res);

        res = await commands.AddUsers();
        //Console.WriteLine(res);

        var bobTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(api, Users.Bob, Symbols.MSFT, 100m, 0.01m, 100);
        });

        var aliceTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(api, Users.Alice, Symbols.MSFT, 100m, 0.01m, 100);
        });

        await Task.WhenAll(aliceTrading, bobTrading);

        //var ob = await api.GetOrderBook(Symbols.MSFT.Id);
        //OrderBookView.Render(ob, "Random walk");

        var trades = await api.GetTrades();
        TradesView.Render(trades);
    }
}
