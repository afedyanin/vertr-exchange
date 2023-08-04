using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertr.Infrastructure.Common.Contracts;
using Vertr.Infrastructure.Common.Implementation;
using Vertr.Infrastructure.Common.Messaging;
using Vertr.OrderMatching.Application.Commands.BuySell;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Factories;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Tests
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddLogging(configure => configure.AddConsole());

            serviceCollection
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<BuySellCommand>());

            serviceCollection
                .AddSingleton<IOrderRepository, OrderRepository>();

            serviceCollection
                .AddSingleton<IOrderFactory, OrderFactory>();

            serviceCollection
                .AddSingleton<ITimeService, RealTimeService>();

            serviceCollection
                .AddSingleton<IEntityIdGenerator<Guid>, GuidEntityIdGenerator>();

            serviceCollection
                .AddSingleton<ITopicProvider<Order>, ChannelBasedTopicProvider<Order>>();

            return serviceCollection;
        }
    }
}