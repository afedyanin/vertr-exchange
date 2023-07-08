using Microsoft.Extensions.DependencyInjection;
using Vertr.ExchCore.Domain.Abstractions;

namespace Vertr.ExchCore.Infrastructure.Disruptor;

public static class DisruptorRegistrar
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IOrderCommandPublisher, OrderCommandDisruptorService>();
        return services;
    }
}
