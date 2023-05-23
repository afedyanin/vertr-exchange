using Vertr.OrderMatching.Application.Commands.Buy;

namespace Vertr.OrderMatching.Application.Tests.Commands
{
    public class BuyCommandTests : ServiceProviderTestBase
    {
        [SetUp]
        public void Setup()
        {
            OrderTopicProvider.GetOrAdd("SBER");
        }

        [Test]
        public async Task CanCreateBuyMarketCommand()
        {
            var ownerId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var buyMarket = new BuyMarketCommand(correlationId, ownerId, "SBER", 12);
            var res = await Mediator.Send(buyMarket);

            Assert.That(res, Is.Not.Null);
            Assert.That(!res.HasErrors);
            Assert.That(res.OrderId, Is.Not.EqualTo(Guid.Empty));
        }

        [Test]
        public async Task CanGetOrderFromRepository()
        {
            var ownerId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var buyMarket = new BuyMarketCommand(correlationId, ownerId, "SBER", 38);
            var res = await Mediator.Send(buyMarket);
            var order = OrderRepository.GetById(res.OrderId);

            Assert.Multiple(() =>
            {
                Assert.That(order, Is.Not.Null);
                Assert.That(order!.Instrument, Is.EqualTo("SBER"));
                Assert.That(order!.CorrelationId, Is.EqualTo(correlationId));
                Assert.That(order!.OwnerId, Is.EqualTo(ownerId));
                Assert.That(order!.Qty, Is.EqualTo(38));
            });
        }

        [Test]
        public async Task CanGetOrderFromTopicConsumer()
        {
            var topic = OrderTopicProvider.GetOrAdd("SBER");

            var ownerId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var buyMarket = new BuyMarketCommand(correlationId, ownerId, "SBER", 38);
            var res = await Mediator.Send(buyMarket);
            var consumedOrder = await topic.Consume();

            Assert.Multiple(() =>
            {
                Assert.That(consumedOrder, Is.Not.Null);
                Assert.That(consumedOrder.Instrument, Is.EqualTo("SBER"));
                Assert.That(consumedOrder.CorrelationId, Is.EqualTo(correlationId));
                Assert.That(consumedOrder.OwnerId, Is.EqualTo(ownerId));
                Assert.That(consumedOrder.Qty, Is.EqualTo(38));
            });
        }
    }
}
