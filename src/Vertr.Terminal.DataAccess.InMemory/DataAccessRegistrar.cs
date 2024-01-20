using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Vertr.Terminal.DataAccess.InMemory.Repositories;
using Vertr.Terminal.Domain.Abstractions;

[assembly: InternalsVisibleTo("Vertr.Terminal.DataAccess.InMemory.Tests")]

namespace Vertr.Terminal.DataAccess.InMemory;

public static class DataAccessRegistrar
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<IOrderBookSnapshotsRepository, OrderBookSnapshotsRepository>();
        services.AddSingleton<ITradeEventsRepository, TradeEventsRepository>();
        services.AddSingleton<IOrderRepository, OrdersRepository>();
        services.AddSingleton<IMarketDataRepository, MarketDataRepository>();
        services.AddSingleton<IPortfolioRepository, PortfolioRepository>();

        return services;
    }
}
