using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.EventHandlers;
using Vertr.Exchange.Api.Generators;
using Vertr.Exchange.Core;
using Vertr.Exchange.Core.EventHandlers;

namespace Vertr.Exchange.Api;

public static class ExchangeApiRegistrar
{
    public static IServiceCollection AddExchangeApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RequestCompletionProcessor>();
        serviceCollection.AddSingleton<IRequestAwaitingService, RequestAwatingService>();
        serviceCollection.AddSingleton<IOrderIdGenerator, OrderIdGenerator>();
        serviceCollection.AddSingleton<ITimestampGenerator, TimestampGenerator>();
        serviceCollection.AddSingleton<IExchangeApi, ExchangeApi>();
        serviceCollection.AddExchangeCore();

        return serviceCollection;
    }
}
