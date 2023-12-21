using MediatR;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.Application.Commands.Orders;

namespace Vertr.Terminal.Server.Strategies;

public record RandomWalkStrategyParams(
    long UserId,
    int SymbolId,
    decimal BasePrice,
    decimal PriceDelta = .01m,
    int OrdersCount = 10);

public class RandomWalkStrategy(
    IMediator mediator,
    RandomWalkStrategyParams strategyParams)
{
    private readonly RandomWalkStrategyParams _strategyParams = strategyParams;
    private readonly IMediator _mediator = mediator;
    private const int _orderCommadsDelay = 1;

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        var nextPrice = _strategyParams.BasePrice;

        for (var i = 0; i < _strategyParams.OrdersCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            nextPrice = NextRandomPrice(nextPrice, _strategyParams.PriceDelta);
            var size = NextRandomQty();
            var placeRequest = new PlaceRequest
            {
                PlaceOrderRequest = new PlaceOrderRequest
                {
                    UserId = _strategyParams.UserId,
                    Symbol = _strategyParams.SymbolId,
                    Price = nextPrice,
                    Size = Math.Abs(size),
                    Action = size > 0 ? OrderAction.BID : OrderAction.ASK,
                    OrderType = nextPrice == decimal.Zero ? OrderType.IOC : OrderType.GTC,
                }
            };

            await _mediator.Send(placeRequest);
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
