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
}
