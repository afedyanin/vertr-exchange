using Microsoft.Extensions.DependencyInjection;
using Vertr.Terminal.DataAccess.InMemory.Repositories;
using Vertr.Terminal.Domain.Abstractions;

namespace Vertr.Terminal.DataAccess.InMemory;
public static class DataAccessRegistrar
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddSingleton<IOrderBookSnapshotsRepository, OrderBookSnapshotsRepository>();
        services.AddSingleton<ITradeEventsRepository, TradeEventsRepository>();
        services.AddSingleton<IOrderRepository, OrdersRepository>();

        return services;
    }
}
