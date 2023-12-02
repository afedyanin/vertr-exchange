using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.EventHandlers;
using Vertr.Exchange.Api.Generators;
using Vertr.Exchange.Core.EventHandlers;

namespace Vertr.Exchange.Api;

public static class ExchangeApiRegistrar
{
    public static IServiceCollection AddExchangeApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, SimpleMessageProcessor>();
        serviceCollection.AddSingleton<IOrderIdGenerator, OrderIdGenerator>();
        serviceCollection.AddSingleton<ITimestampGenerator, TimestampGenerator>();
        serviceCollection.AddSingleton<IExchangeApi, ExchangeApi>();

        return serviceCollection;
    }
}
