using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrderBookL2MarketDataTests
{
    [Test]
    public void CanGetL2MarketData()
    {
        var orderBook = new OrderBook();
        var snapshot = orderBook.GetL2MarketDataSnapshot(100);

        Assert.That(snapshot, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(snapshot.AskSize, Is.EqualTo(0));
            Assert.That(snapshot.BidSize, Is.EqualTo(0));
            Assert.That(snapshot.AskPrices, Is.Empty);
            Assert.That(snapshot.BidPrices, Is.Empty);
            Assert.That(snapshot.AskVolumes, Is.Empty);
            Assert.That(snapshot.BidVolumes, Is.Empty);
            Assert.That(snapshot.AskOrders, Is.Empty);
            Assert.That(snapshot.BidOrders, Is.Empty);
        });


    }

    // TODO: Wrtite test
    [Test]
    public void L2MarketDataWithSingleBid()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45.98M, 783);
        orderBook.AddOrder(bid);

        var snapshot = orderBook.GetL2MarketDataSnapshot(100);

        Assert.That(snapshot, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(snapshot.AskSize, Is.EqualTo(0));
            Assert.That(snapshot.BidSize, Is.EqualTo(0));
            Assert.That(snapshot.AskPrices, Is.Empty);
            Assert.That(snapshot.BidPrices, Is.Empty);
            Assert.That(snapshot.AskVolumes, Is.Empty);
            Assert.That(snapshot.BidVolumes, Is.Empty);
            Assert.That(snapshot.AskOrders, Is.Empty);
            Assert.That(snapshot.BidOrders, Is.Empty);
        });
    }

    // TODO: Wrtite test
    [Test]
    public void L2MarketDataWithSingleAsk()
    {
        var orderBook = new OrderBook();
        var ask = OrderStub.CreateBidOrder(78.12M, 435);
        orderBook.AddOrder(ask);

        var snapshot = orderBook.GetL2MarketDataSnapshot(100);

        Assert.That(snapshot, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(snapshot.AskSize, Is.EqualTo(0));
            Assert.That(snapshot.BidSize, Is.EqualTo(0));
            Assert.That(snapshot.AskPrices, Is.Empty);
            Assert.That(snapshot.BidPrices, Is.Empty);
            Assert.That(snapshot.AskVolumes, Is.Empty);
            Assert.That(snapshot.BidVolumes, Is.Empty);
            Assert.That(snapshot.AskOrders, Is.Empty);
            Assert.That(snapshot.BidOrders, Is.Empty);
        });
    }
}
