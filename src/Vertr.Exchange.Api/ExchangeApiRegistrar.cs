using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.EventHandlers;
using Vertr.Exchange.Infrastructure;
using Vertr.Exchange.Infrastructure.EventHandlers;

namespace Vertr.Exchange.Api;

public static class ExchangeApiRegistrar
{
    public static IServiceCollection AddExchangeApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RequestCompletionProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, SimpleMessageProcessor>();
        serviceCollection.AddSingleton<IRequestAwaitingService, RequestAwatingService>();
        serviceCollection.AddSingleton<IExchangeApi, ExchangeApi>();
        serviceCollection.AddExchangeCore();

        return serviceCollection;
    }
}
