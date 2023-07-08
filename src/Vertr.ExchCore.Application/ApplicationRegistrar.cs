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
        services.AddSingleton<IOrderCommandSubscriber, OrderEventNotificator>();
        services.AddSingleton<IOrderCommandSubscriber, OrderJournaling>();
        services.AddSingleton<IOrderCommandSubscriber, OrderMatching>();
        services.AddSingleton<IOrderCommandSubscriber, OrderReplication>();

        return services;
    }
}
