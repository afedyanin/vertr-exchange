using Microsoft.Extensions.DependencyInjection;
using Vertr.ExchCore.Application.Api;
using Vertr.ExchCore.Application.Subscribers;
using Vertr.ExchCore.Domain.Abstractions;

namespace Vertr.ExchCore.Application;

public static class ApplicationRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IOrderManagementApi, OrderManagementApi>();
        services.AddSingleton<IOrderCommandSubscriber, EventsProcessor>();
        services.AddSingleton<IOrderCommandSubscriber, SampleOrderJournaling>();
        services.AddSingleton<IOrderCommandSubscriber, MatchingEngineRouter>();
        services.AddSingleton<IOrderCommandSubscriber, SampleOrderReplication>();

        return services;
    }
}
