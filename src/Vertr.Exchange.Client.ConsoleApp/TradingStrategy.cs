using Vert.Exchange.Client.Host.ApiClient;
using Vertr.Exchange.Client.ConsoleApp.StaticData;

namespace Vertr.Exchange.Client.ConsoleApp;
internal static class TradingStrategy
{
    public static async Task RandomWalkTrading(
        IHostApiClient api,
        User user,
        Symbol symbol,
        decimal basePrice,
        decimal priceDelta = .01m,
        int ordersCount = 10)
    {
        var nextPrice = basePrice;
        for (var i = 0; i < ordersCount; i++)
        {
            nextPrice = NextRandomPrice(nextPrice, priceDelta);
            var res = await Commands.PlaceOrder(api, user, symbol, nextPrice, NextRandomQty());
            Console.WriteLine(res);
            await Task.Delay(200);
        }
    }
    private static decimal NextRandomPrice(decimal baseParice, decimal delta)
    {
        var deltaPrice = (delta * RandomSign()) + 1;
        return baseParice * deltaPrice;
    }

    private static int NextRandomQty()
        => Random.Shared.Next(10) * RandomSign();

    private static int RandomSign()
        => Random.Shared.NextDouble() >= .51 ? -1 : 1;
}
