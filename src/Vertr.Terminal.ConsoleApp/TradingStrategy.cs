using Refit;
using Vertr.Terminal.ApiClient;
using Vertr.Terminal.ConsoleApp.StaticData;

namespace Vertr.Terminal.ConsoleApp;
internal static class TradingStrategy
{
    public static async Task RandomWalkTrading(
        User user,
        Symbol symbol,
        decimal basePrice,
        decimal priceDelta = .01m,
        int ordersCount = 10)
    {
        var client = RestService.For<ITerminalApiClient>("http://localhost:5010");
        var commands = new Commands(client);
        var nextPrice = basePrice;

        for (var i = 0; i < ordersCount; i++)
        {
            nextPrice = NextRandomPrice(nextPrice, priceDelta);
            _ = await commands.PlaceOrder(user, symbol, nextPrice, NextRandomQty());
            await Task.Delay(1);
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
