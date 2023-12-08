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
        var apiUrl = "http://localhost:5010";
        var api = RestService.For<ITerminalApiClient>(apiUrl);

        var res = await Commands.Reset(api);
        //Console.WriteLine(res);

        res = await Commands.AddSymbols(api);
        //Console.WriteLine(res);

        res = await Commands.AddUsers(api);
        //Console.WriteLine(res);

        var bobTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(apiUrl, Users.Bob, Symbols.MSFT, 100m, 0.01m, 100);
        });

        var aliceTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(apiUrl, Users.Alice, Symbols.MSFT, 100m, 0.01m, 100);
        });

        await Task.WhenAll(aliceTrading, bobTrading);

        //var ob = await api.GetOrderBook(Symbols.MSFT.Id);
        //OrderBookView.Render(ob, "Random walk");

        var trades = await api.GetTrades();
        TradesView.Render(trades);
    }
}
