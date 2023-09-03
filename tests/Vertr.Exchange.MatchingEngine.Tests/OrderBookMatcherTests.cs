using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrderBookMatcherTests
{
    [Test]
    public void CanMatchOrder()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45M, 2);
        orderBook.AddOrder(bid);

        var res = orderBook.TryMatchInstantly(OrderAction.ASK, 44M, 5);
        Assert.Multiple(() =>
        {
            Assert.That(res.Filled, Is.EqualTo(2));
            Assert.That(res.TradeEvents.Count, Is.EqualTo(1));
            Assert.That(bid.Completed, Is.True);
        });
    }

    [Test]
    public void CanMatchPreFilledOrder()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45M, 2);
        orderBook.AddOrder(bid);

        var res = orderBook.TryMatchInstantly(OrderAction.ASK, 44M, 5, 1);
        Assert.Multiple(() =>
        {
            Assert.That(res.Filled, Is.EqualTo(3));
            Assert.That(res.TradeEvents.Count, Is.EqualTo(1));
            Assert.That(bid.Completed, Is.True);
        });
    }

    [Test]
    public void CanMatchFullFilledOrder()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45M, 43);
        orderBook.AddOrder(bid);

        var res = orderBook.TryMatchInstantly(OrderAction.ASK, 44M, 11, 27);
        Assert.Multiple(() =>
        {
            Assert.That(res.Filled, Is.EqualTo(27));
            Assert.That(res.TradeEvents.Count, Is.EqualTo(0));
            Assert.That(bid.Completed, Is.False);
        });
    }


    [Test]
    public void CanMatchSeveralOrders()
    {
        var orderBook = new OrderBook();

        var ask1 = OrderStub.CreateAskOrder(63.17M, 7);
        var ask2 = OrderStub.CreateAskOrder(63.19M, 3);
        var ask3 = OrderStub.CreateAskOrder(63.11M, 6);

        orderBook.AddOrder(ask1);
        orderBook.AddOrder(ask2);
        orderBook.AddOrder(ask3);

        var res = orderBook.TryMatchInstantly(OrderAction.BID, 63.18M, 8);
        Assert.Multiple(() =>
        {
            Assert.That(res.Filled, Is.EqualTo(8));
            Assert.That(res.TradeEvents.Count(), Is.EqualTo(2));
            Assert.That(ask1.Filled, Is.EqualTo(2));
            Assert.That(ask2.Filled, Is.EqualTo(0));
            Assert.That(ask3.Completed, Is.True);
        });
    }

    [Test]
    public void CanMatchSeveralOrdersPartially()
    {
        var orderBook = new OrderBook();

        var ask1 = OrderStub.CreateAskOrder(13.01M, 5);
        var ask2 = OrderStub.CreateAskOrder(13.05M, 5);
        var ask3 = OrderStub.CreateAskOrder(13.07M, 5);

        orderBook.AddOrder(ask1);
        orderBook.AddOrder(ask2);
        orderBook.AddOrder(ask3);

        var res = orderBook.TryMatchInstantly(OrderAction.BID, 13.06M, 12, 3);
        Assert.Multiple(() =>
        {
            Assert.That(res.Filled, Is.EqualTo(12));
            Assert.That(res.TradeEvents.Count(), Is.EqualTo(2));
            Assert.That(ask1.Completed, Is.True);
            Assert.That(ask2.Completed, Is.False);
            Assert.That(ask3.Completed, Is.False);
        });
    }
}
