using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.OrderBooks;

namespace Vertr.Exchange.Domain.MatchingEngine;
public static class MatchingEngineRegistrar
{
    public static IServiceCollection AddMatchingEngine(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderBookProvider, OrderBookProvider>();
        serviceCollection.AddSingleton<IOrderMatchingEngine, OrderMatchingEngine>();
        serviceCollection.AddOptions<MatchingEngineConfiguration>();
        serviceCollection.AddSingleton(CreateOrderBook);
        return serviceCollection;
    }

    private static IOrderBook CreateOrderBook() => new OrderBook();
}
