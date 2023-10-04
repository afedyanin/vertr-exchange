using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.EventHandlers;
using Vertr.Exchange.Core.EventHandlers;

namespace Vertr.Exchange.Api;

public static class ExchangeApiAdapterRegistrar
{
    public static IServiceCollection AddMessageProcessor(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, SimpleMessageProcessor>();

        return serviceCollection;
    }
}
