using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using Vertr.Exchange.Contracts.Enums;
using Vertr.Exchange.Contracts.Requests;
using static Vertr.Exchange.SignalRClient.ConsoleApp.StaticContext;

namespace Vertr.Exchange.SignalRClient.ConsoleApp;

internal sealed class Program
{
    public static async Task Main()
    {
        Console.WriteLine("Starting...");

        var config = GetConfiguration();
        var sp = ServicePorviderBuilder.BuildServiceProvider(config);
        var api = sp.GetRequiredService<IExchangeApiClient>();

        await Task.Delay(2000);

        Console.WriteLine("Sending add symbols command...");

        var addSymbolsResult = await api.AddSymbols(
            new AddSymbolsRequest
            {
                Symbols = Symbols.AllSymbolSpecs
            });

        Console.WriteLine($"Add symbols command result: {addSymbolsResult}");
        /*
        var addAccountsResult = await api.AddAccounts(
            new AddAccountsRequest
            {
                UserAccounts = UserAccounts.All
            });

        Console.WriteLine($"Add accounts command result: {addAccountsResult}");

        var askOrderResult = await api.PlaceOrder(
            new PlaceOrderRequest
            {
                OrderType = OrderType.GTC,
                Action = OrderAction.ASK,
                UserId = Users.Alice.Id,
                Price = 120.45m,
                Size = 10,
                Symbol = Symbols.MSFT.Id
            });

        Console.WriteLine($"Ask order command result: {askOrderResult}");

        var bidOrderResult = await api.PlaceOrder(
            new PlaceOrderRequest
            {
                OrderType = OrderType.GTC,
                Action = OrderAction.BID,
                UserId = Users.Bob.Id,
                Price = 123.56m,
                Size = 7,
                Symbol = Symbols.MSFT.Id
            });

        Console.WriteLine($"Bid order command result: {bidOrderResult}");
        */

        Console.WriteLine("Ending...");

    }

    private static IConfiguration GetConfiguration()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        return config;
    }
}
