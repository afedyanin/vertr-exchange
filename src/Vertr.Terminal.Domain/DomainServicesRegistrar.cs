using Microsoft.Extensions.DependencyInjection;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Domain.MarketData;
using Vertr.Terminal.Domain.OrderManagement;
using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.Domain;

public static class DomainServicesRegistrar
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IMarketDataService, MarketDataService>();
        services.AddScoped<IOrderEventService, OrderEventService>();
        services.AddScoped<IPortolioService, PortolioService>();

        return services;
    }
}
