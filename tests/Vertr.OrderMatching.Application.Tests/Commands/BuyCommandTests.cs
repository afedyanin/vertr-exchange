using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Vertr.OrderMatching.Application.Commands.Buy;

namespace Vertr.OrderMatching.Application.Tests.Commands
{
    public class BuyCommandTests
    {
        private ServiceProvider _serviceProvider;
        private ILogger<BuyCommandTests> _logger;
        private IMediator _mediator;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddServices();
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _mediator = _serviceProvider.GetService<IMediator>()!;
        }

        [SetUp]
        public void Setup()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<BuyCommandTests>();
        }

        [Test]
        public async Task CanCreateBuyMarketCommand()
        {
            var buyMarket = new BuyMarketCommand(Guid.NewGuid(), "SBER", 12);
            _logger.LogInformation("New buy market command created: {buyMarket}", buyMarket);

            await _mediator.Send(buyMarket);

            Assert.Pass();
        }
    }
}
