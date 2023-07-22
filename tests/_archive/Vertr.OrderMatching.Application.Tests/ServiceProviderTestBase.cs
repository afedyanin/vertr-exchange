using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Tests
{
    public abstract class ServiceProviderTestBase
    {
        protected ServiceProvider ServiceProvider { get; }

        protected IMediator Mediator => ServiceProvider.GetService<IMediator>()!;

        protected IOrderRepository OrderRepository => ServiceProvider.GetService<IOrderRepository>()!;

        protected ITopicProvider<Order> OrderTopicProvider => ServiceProvider.GetService<ITopicProvider<Order>>()!;

        protected ServiceProviderTestBase()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddServices();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
