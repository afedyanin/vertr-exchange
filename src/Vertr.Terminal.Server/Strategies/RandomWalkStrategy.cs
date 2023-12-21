namespace Vertr.Terminal.Server.Strategies;

public record RandomWalkStrategyParams(
    int UserId,
    int SymbolId,
    decimal BasePrice,
    decimal PriceDelta = .01m,
    int OrdersCount = 10);


public class RandomWalkStrategy(
    RandomWalkStrategyParams strategyParams)
{
    private readonly RandomWalkStrategyParams _strategyParams = strategyParams;
    private const int _orderCommadsDelay = 1;

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var nextPrice = _strategyParams.BasePrice;

        for (var i = 0; i < _strategyParams.OrdersCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            nextPrice = NextRandomPrice(nextPrice, _strategyParams.PriceDelta);
            _ = await commands.PlaceOrder(user, symbol, nextPrice, NextRandomQty());

            await Task.Delay(_orderCommadsDelay, cancellationToken);
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
