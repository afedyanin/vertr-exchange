using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Application.Tests
{
    public abstract class ServiceProviderTestBase
    {
        protected ServiceProvider ServiceProvider { get; }

        protected IMediator Mediator => ServiceProvider.GetService<IMediator>()!;

        protected IOrderRepository OrderRepository => ServiceProvider.GetService<IOrderRepository>()!;

        protected ServiceProviderTestBase()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddServices();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
