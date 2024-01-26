using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Application.Commands.Api;
using Vertr.Exchange.Application.Core;
using Vertr.Exchange.Application.EventHandlers;
using Vertr.Exchange.Application.Generators;

namespace Vertr.Exchange.Application;

public static class ExchangeApiRegistrar
{
    public static IServiceCollection AddExchangeCommandsApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IOrderCommandEventHandler, SimpleMessageProcessor>();
        serviceCollection.AddSingleton<IOrderIdGenerator, OrderIdGenerator>();
        serviceCollection.AddSingleton<ITimestampGenerator, TimestampGenerator>();
        serviceCollection.AddSingleton<IExchangeCommandsApi, ExchangeCommandsApi>();

        serviceCollection.AddSingleton<IExchangeCoreService, ExchangeCoreService>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RiskEnginePreProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, RiskEnginePostProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, MatchingEngineProcessor>();
        serviceCollection.AddSingleton<IOrderCommandEventHandler, LoggingProcessor>();

        return serviceCollection;
    }
}
