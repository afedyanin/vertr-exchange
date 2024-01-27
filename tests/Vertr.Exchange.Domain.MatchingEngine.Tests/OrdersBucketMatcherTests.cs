using Vertr.Exchange.Domain.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.Domain.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrdersBucketMatcherTests
{
    [TestCase(100, 0, 56)]
    [TestCase(112, 62, 40)]
    [TestCase(112, 62, 82)]
    public void CanMatchWithSingleOrder(long orderSize, long filled, long toCollect)
    {
        var price = 782M;
        var ob = new OrdersBucket(price);
        var bid = OrderStub.CreateBidOrder(price, orderSize, filled);
        ob.Put(bid);

        var res = ob.Match(toCollect);

        Assert.That(res, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(res.Volume, Is.EqualTo(Math.Min(toCollect, orderSize - filled)));
            Assert.That(bid.Remaining, Is.EqualTo(Math.Max(0L, orderSize - filled - toCollect)));
            Assert.That(res.TradeEvents, Is.Not.Empty);
            Assert.That(res.TradeEvents, Has.Length.EqualTo(1));
            Assert.That(res.TradeEvents[0].MatchedOrderId, Is.EqualTo(bid.OrderId));
            Assert.That(res.TradeEvents[0].MatchedOrderUid, Is.EqualTo(bid.Uid));
            Assert.That(res.TradeEvents[0].Price, Is.EqualTo(price));
            Assert.That(res.TradeEvents[0].Size, Is.EqualTo(res.Volume));
        });
    }

    [TestCase(100, 50)]
    [TestCase(200, 200)]
    [TestCase(300, 350)]
    public void CanCompleteOrderWithSingleOrder(long orderSize, long toCollect)
    {
        var price = 0.2568M;
        var ob = new OrdersBucket(price);
        var bid = OrderStub.CreateBidOrder(price, orderSize);
        ob.Put(bid);

        var res = ob.Match(toCollect);

        Assert.That(res, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(res.TradeEvents, Has.Length.EqualTo(1));
            Assert.That(res.TradeEvents[0].Price, Is.EqualTo(price));
            Assert.That(res.TradeEvents[0].Size, Is.EqualTo(res.Volume));
            Assert.That(res.TradeEvents[0].ActiveOrderCompleted, Is.EqualTo(toCollect <= orderSize));
            Assert.That(res.TradeEvents[0].MatchedOrderCompleted, Is.EqualTo(toCollect >= orderSize));
        });
    }

    [TestCase(100, 50)]
    [TestCase(200, 200)]
    [TestCase(300, 350)]
    public void CanFillOrdersToRemove(long orderSize, long toCollect)
    {
        var price = 456.2568M;
        var ob = new OrdersBucket(price);
        var bid = OrderStub.CreateBidOrder(price, orderSize);
        ob.Put(bid);
        var shouldBeRemoved = orderSize <= toCollect;

        var res = ob.Match(toCollect);

        Assert.That(res, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(res.OrdersToRemove, Has.Length.EqualTo(shouldBeRemoved ? 1 : 0));

            if (shouldBeRemoved)
            {
                Assert.That(res.OrdersToRemove[0], Is.EqualTo(bid.OrderId));
            }
        });
    }


    [TestCase(100, 50, 20, 1)]
    [TestCase(10, 50, 30, 2)]
    [TestCase(13, 27, 40, 2)]
    public void CanMatchWithManyOrders(long size1, long size2, long toCollect, int eventsCount)
    {
        var price = 456.2568M;
        var ob = new OrdersBucket(price);
        var ask1 = OrderStub.CreateAskOrder(price, size1);
        ob.Put(ask1);
        var ask2 = OrderStub.CreateAskOrder(price, size2);
        ob.Put(ask2);

        var res = ob.Match(toCollect);

        Assert.That(res, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(ob.TotalVolume, Is.EqualTo(size1 + size2 - toCollect));
            Assert.That(res.Volume, Is.EqualTo(toCollect));
            Assert.That(res.TradeEvents, Is.Not.Empty);
            Assert.That(res.TradeEvents, Has.Length.EqualTo(eventsCount));

            Assert.That(res.TradeEvents[0].MatchedOrderId, Is.EqualTo(ask1.OrderId));
            Assert.That(res.TradeEvents[0].MatchedOrderUid, Is.EqualTo(ask1.Uid));
            Assert.That(res.TradeEvents[0].Price, Is.EqualTo(price));

            if (eventsCount > 1)
            {
                Assert.That(res.TradeEvents[1].MatchedOrderId, Is.EqualTo(ask2.OrderId));
                Assert.That(res.TradeEvents[1].MatchedOrderUid, Is.EqualTo(ask2.Uid));
                Assert.That(res.TradeEvents[1].Price, Is.EqualTo(price));
                Assert.That(res.TradeEvents[0].Size + res.TradeEvents[1].Size, Is.EqualTo(res.Volume));
            }
            else
            {
                Assert.That(res.TradeEvents[0].Size, Is.EqualTo(res.Volume));
            }
        });
    }
}
