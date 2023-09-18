using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Infrastructure.EventHandlers;

namespace Vertr.Exchange.Infrastructure;
public static class ExchangeCoreRegistrar
{
    public static IServiceCollection AddExchangeCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, LoggingProcessor>();
        serviceCollection.AddSingleton<IExchangeCoreService, ExchangeCoreService>();
        return serviceCollection;
    }

    public static IServiceCollection UseRiskEngine(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RiskEnginePreProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RiskEnginePostProcessor>();
        return serviceCollection;
    }

    public static IServiceCollection UseMatchingEngine(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, MatchingEngineProcessor>();
        return serviceCollection;
    }
}
