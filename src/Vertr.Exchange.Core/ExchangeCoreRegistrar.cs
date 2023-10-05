using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Core.EventHandlers;

namespace Vertr.Exchange.Core;
public static class ExchangeCoreRegistrar
{
    public static IServiceCollection AddExchangeCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExchangeCoreService, ExchangeCoreService>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RiskEnginePreProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RiskEnginePostProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, MatchingEngineProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, LoggingProcessor>();
        return serviceCollection;
    }
}
