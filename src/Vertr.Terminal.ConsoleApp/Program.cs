using Refit;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        var api = RestService.For<ITerminalApiClient>("http://localhost:5010");

        var res = await Commands.Reset(api);
        // Console.WriteLine(res);

        res = await Commands.AddSymbols(api);
        // Console.WriteLine(res);

        res = await Commands.AddUsers(api);
        // Console.WriteLine(res);

        var bobTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(api, Users.Bob, Symbols.MSFT, 100m, 0.01m, 100);
        });

        var aliceTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(api, Users.Alice, Symbols.MSFT, 100m, 0.01m, 100);
        });

        await Task.WhenAll(aliceTrading, bobTrading);

        // var ob = await exchApi.GetOrderBook(Symbols.MSFT.Id);
        // OrderBookView.Render(ob, "Random walk");

        var trades = await api.GetTrades();

        foreach (var trade in trades)
        {
            Console.WriteLine(trade);
        }
    }
}
