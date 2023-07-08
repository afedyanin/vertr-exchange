using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Infrastructure.Disruptor.Configuration;

namespace Vertr.ExchCore.Infrastructure.Disruptor;

public static class DisruptorRegistrar
{
    public static IServiceCollection AddDisruptor(this IServiceCollection services)
    {
        services.AddOptions<DisruptorOptions>(DisruptorOptions.Disruptor);
        services.AddSingleton<IOrderCommandPublisher, OrderCommandDisruptorService>();
        return services;
    }
}
