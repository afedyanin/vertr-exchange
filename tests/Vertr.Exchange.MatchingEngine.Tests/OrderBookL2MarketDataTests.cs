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
            Assert.That(snapshot.AskPrices, Is.Empty);
            Assert.That(snapshot.AskVolumes, Is.Empty);
            Assert.That(snapshot.AskOrders, Is.Empty);

            Assert.That(snapshot.BidSize, Is.EqualTo(1)); // всего бакетов
            Assert.That(snapshot.BidPrices.Count, Is.EqualTo(1));
            Assert.That(snapshot.BidPrices[0], Is.EqualTo(bid.Price)); // цена каждого бакета
            Assert.That(snapshot.BidVolumes.Count, Is.EqualTo(1));
            Assert.That(snapshot.BidVolumes[0], Is.EqualTo(bid.Size)); // открытое кол-во в каждом бакете
            Assert.That(snapshot.BidOrders.Count, Is.EqualTo(1));
            Assert.That(snapshot.BidOrders[0], Is.EqualTo(1)); // кол-во ордеров в каждом бакете
        });
    }

    [Test]
    public void L2MarketDataWithManyBids()
    {
        var orderBook = new OrderBook();

        var bid1 = OrderStub.CreateBidOrder(45.98M, 783);
        var bid2 = OrderStub.CreateBidOrder(36.87M, 52);
        orderBook.AddOrder(bid1);
        orderBook.AddOrder(bid2);

        var snapshot = orderBook.GetL2MarketDataSnapshot(100);

        Assert.That(snapshot, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(snapshot.BidSize, Is.EqualTo(2));
            Assert.That(snapshot.BidPrices.Count, Is.EqualTo(2));
            Assert.That(snapshot.BidPrices[0], Is.EqualTo(bid1.Price));
            Assert.That(snapshot.BidPrices[1], Is.EqualTo(bid2.Price));
            Assert.That(snapshot.BidVolumes.Count, Is.EqualTo(2));
            Assert.That(snapshot.BidVolumes[0], Is.EqualTo(bid1.Size));
            Assert.That(snapshot.BidVolumes[1], Is.EqualTo(bid2.Size));
            Assert.That(snapshot.BidOrders.Count, Is.EqualTo(2));
            Assert.That(snapshot.BidOrders[0], Is.EqualTo(1));
            Assert.That(snapshot.BidOrders[1], Is.EqualTo(1));
        });
    }

    [Test]
    public void L2MarketDataWithSingleAsk()
    {
        var orderBook = new OrderBook();
        var ask = OrderStub.CreateAskOrder(78.12M, 435);
        orderBook.AddOrder(ask);

        var snapshot = orderBook.GetL2MarketDataSnapshot(100);

        Assert.That(snapshot, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(snapshot.BidSize, Is.EqualTo(0));
            Assert.That(snapshot.BidPrices, Is.Empty);
            Assert.That(snapshot.BidVolumes, Is.Empty);
            Assert.That(snapshot.BidOrders, Is.Empty);

            Assert.That(snapshot.AskSize, Is.EqualTo(1));
            Assert.That(snapshot.AskPrices.Count, Is.EqualTo(1));
            Assert.That(snapshot.AskPrices[0], Is.EqualTo(ask.Price));
            Assert.That(snapshot.AskVolumes.Count, Is.EqualTo(1));
            Assert.That(snapshot.AskVolumes[0], Is.EqualTo(ask.Size));
            Assert.That(snapshot.AskOrders.Count, Is.EqualTo(1));
            Assert.That(snapshot.AskOrders[0], Is.EqualTo(1));
        });
    }
}
