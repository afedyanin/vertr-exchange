using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vertr.Terminal.ExchangeClient.Awaiting;
using Vertr.Terminal.ExchangeClient.Configuration;
using Vertr.Terminal.ExchangeClient.Providers;
using Vertr.Terminal.ExchangeClient.Streams;

namespace Vertr.Terminal.ExchangeClient;

public static class ExchangeApiClientRegistrar
{
    public static IServiceCollection AddExchangeApi(this IServiceCollection services, IConfiguration configuration)
    {
        var exchangeOptions = new ExchangeConfiguration();
        configuration.GetSection(ExchangeConfiguration.SectionName).Bind(exchangeOptions);

        services.AddSingleton<IHubConnectionProvider, HubConnectionProvider>();
        services.AddSingleton<ICommandAwaitingService, CommandAwaitingService>();

        services.AddHostedService<CommandResultStream>();
        services.AddHostedService<OrderBooksStream>();
        services.AddHostedService<TradeEventStream>();
        services.AddHostedService<ReduceEventStream>();
        services.AddHostedService<RejectEventStream>();

        return services;
    }
}
