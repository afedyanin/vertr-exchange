using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp;
internal static class TradingStrategy
{
    public static async Task RandomWalkTrading(
        ITerminalApiClient client,
        User user,
        Symbol symbol,
        decimal basePrice,
        decimal priceDelta = .01m,
        int ordersCount = 10)
    {
        var commands = new Commands(client);
        var nextPrice = basePrice;
        for (var i = 0; i < ordersCount; i++)
        {
            nextPrice = NextRandomPrice(nextPrice, priceDelta);
            _ = await commands.PlaceOrder(user, symbol, nextPrice, NextRandomQty());
            //Console.WriteLine($"{user.Name} {i} => {res}");
            await Task.Delay(10);
        }
    }
    private static decimal NextRandomPrice(decimal baseParice, decimal delta)
    {
        var deltaPrice = 1 + (delta * RandomSign());
        return baseParice * deltaPrice;
    }

    private static int NextRandomQty(int maxValue = 10)
        => Random.Shared.Next(1, maxValue) * RandomSign();

    private static int RandomSign()
        => Random.Shared.NextDouble() >= .51 ? -1 : 1;
}
