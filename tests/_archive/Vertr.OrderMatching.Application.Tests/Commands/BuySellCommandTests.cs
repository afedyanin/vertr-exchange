using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Application.Commands.BuySell;
using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Application.Tests.Commands
{
    public class BuySellCommandTests : ServiceProviderTestBase
    {
        private const string Ticker = "SBER";
        private ITopic<Order> _sberTopic;

        [SetUp]
        public void Setup()
        {
            _sberTopic = OrderTopicProvider.GetOrAdd(Ticker);
        }

        [Test]
        public async Task CanCreateBuyMarketCommand()
        {
            var ownerId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var buyMarket = new BuySellCommand(correlationId, ownerId, Ticker, 12, decimal.Zero, true);
            var res = await Mediator.Send(buyMarket);
            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(!res.HasErrors);
                Assert.That(res.OrderId, Is.Not.EqualTo(Guid.Empty));
            });
        }

        [Test]
        public async Task CanGetOrderFromRepository()
        {
            var ownerId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var buyMarket = new BuySellCommand(correlationId, ownerId, Ticker, 38, decimal.Zero, true);
            var res = await Mediator.Send(buyMarket);
            var order = OrderRepository.GetById(res.OrderId);
            var consumedOrder = await _sberTopic.Consume();

            Assert.Multiple(() =>
            {
                Assert.That(order, Is.Not.Null);
                Assert.That(order!.Ticker, Is.EqualTo(Ticker));
                Assert.That(order!.CorrelationId, Is.EqualTo(correlationId));
                Assert.That(order!.OwnerId, Is.EqualTo(ownerId));
                Assert.That(order!.Qty, Is.EqualTo(38));
            });
        }

        [Test]
        public async Task CanGetOrderFromTopicConsumer()
        {
            var ownerId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var buyMarket = new BuySellCommand(correlationId, ownerId, Ticker, 38, decimal.Zero, true);
            var res = await Mediator.Send(buyMarket);

            var consumedOrder = await _sberTopic.Consume();
            // TODO: So we have order from queue. Need to process it^
            // - GetOrderBook
            // - Match
            // - Create & Send Trades
            // - Create & Send Fulfillments

            Assert.Multiple(() =>
            {
                Assert.That(consumedOrder, Is.Not.Null);
                Assert.That(consumedOrder.Ticker, Is.EqualTo(Ticker));
                Assert.That(consumedOrder.CorrelationId, Is.EqualTo(correlationId));
                Assert.That(consumedOrder.OwnerId, Is.EqualTo(ownerId));
                Assert.That(consumedOrder.Qty, Is.EqualTo(38));
            });
        }
    }
}
