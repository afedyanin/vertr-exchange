using MediatR;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.Application.Commands.Orders;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.Server.Strategies;

public record RandomWalkStrategyParams(
    long UserId,
    int SymbolId,
    decimal BasePrice,
    decimal PriceDelta = .01m,
    int OrdersCount = 10);

public class RandomWalkStrategy(
    IMediator mediator,
    IMarketDataRepository marketDataRepository,
    RandomWalkStrategyParams strategyParams)
{
    private readonly RandomWalkStrategyParams _strategyParams = strategyParams;
    private readonly IMediator _mediator = mediator;
    private readonly IMarketDataRepository _marketDataRepository = marketDataRepository;
    private const int _orderCommadsDelay = 1;

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        // var marketPrice = await GetMarketPrice(_strategyParams.BasePrice);
        var randomPrice = _strategyParams.BasePrice;

        for (var i = 0; i < _strategyParams.OrdersCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            randomPrice = NextRandomPrice(randomPrice, _strategyParams.PriceDelta);

            var size = NextRandomQty();

            var placeRequest = new PlaceRequest
            {
                PlaceOrderRequest = new PlaceOrderRequest
                {
                    UserId = _strategyParams.UserId,
                    Symbol = _strategyParams.SymbolId,
                    Price = randomPrice,
                    Size = Math.Abs(size),
                    Action = size > 0 ? OrderAction.BID : OrderAction.ASK,
                    OrderType = randomPrice == decimal.Zero ? OrderType.IOC : OrderType.GTC,
                }
            };

            await _mediator.Send(placeRequest);
            await Task.Delay(_orderCommadsDelay, cancellationToken);
            // marketPrice = await GetMarketPrice(randomPrice);
        }
    }

    protected async Task<decimal> GetMarketPrice(decimal previousPrice)
    {
        var marketDataItem = await _marketDataRepository.GetBySymbolId(_strategyParams.SymbolId);
        var res = marketDataItem == null ? previousPrice : marketDataItem.Price;

        return res;
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
