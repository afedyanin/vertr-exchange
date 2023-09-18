using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.EventHandlers;
using Vertr.Exchange.Infrastructure;
using Vertr.Exchange.Infrastructure.EventHandlers;

namespace Vertr.Exchange.Api.Tests.Stubs;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBasicEventHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, LoggingProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RequestCompletionProcessor>();

        return serviceCollection;
    }

    public static IServiceCollection AddAwating(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IRequestAwaitingService, RequestAwatingService>();

        return serviceCollection;
    }

    public static IServiceCollection AddExchangeCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExchangeCoreService, ExchangeCoreService>();

        return serviceCollection;
    }

    public static IServiceCollection AddExchangeApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IExchangeApi, ExchangeApi>();

        return serviceCollection;
    }
}
