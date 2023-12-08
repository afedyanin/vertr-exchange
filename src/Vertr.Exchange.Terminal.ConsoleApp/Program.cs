using Refit;
using Vertr.Exchange.Terminal.ApiClient;
using Vertr.Exchange.Terminal.ConsoleApp.StaticData;

namespace Vertr.Exchange.Terminal.ConsoleApp;

public class Program
{
    public static async Task Main()
    {
        var exchApi = RestService.For<IHostApiClient>("http://localhost:5010");

        var res = await Commands.Reset(exchApi);
        // Console.WriteLine(res);

        res = await Commands.AddSymbols(exchApi);
        // Console.WriteLine(res);

        res = await Commands.AddUsers(exchApi);
        // Console.WriteLine(res);

        var bobTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(exchApi, Users.Bob, Symbols.MSFT, 100m, 0.01m, 100);
        });

        var aliceTrading = Task.Run(async () =>
        {
            await TradingStrategy.RandomWalkTrading(exchApi, Users.Alice, Symbols.MSFT, 100m, 0.01m, 100);
        });

        await Task.WhenAll(aliceTrading, bobTrading);

        // var ob = await exchApi.GetOrderBook(Symbols.MSFT.Id);
        // OrderBookView.Render(ob, "Random walk");

        var tradeEvents = await exchApi.GetTradeEvents();

        foreach (var evt in tradeEvents)
        {
            Console.WriteLine(evt);

            foreach (var trade in evt.Trades)
            {
                Console.WriteLine($"--->> {trade}\n");
            }
        }
    }
}
