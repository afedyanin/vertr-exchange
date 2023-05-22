using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Commands.Buy;

namespace Vertr.OrderMatching.Application.Tests
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddLogging(configure => configure.AddConsole());

            serviceCollection
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<BuyLimitCommand>());

            return serviceCollection;
        }
    }
}
