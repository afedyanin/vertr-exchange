using Vertr.Exchange.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrderBookTests
{
    [Test]
    public void CanCreateOrderBook()
    {
        var ob = new OrderBook();

        Assert.That(ob, Is.Not.Null);
        Assert.That(ob.ValidateInternalState(), Is.True);
    }

    [Test]
    public void CanAddOrder()
    {
        var ob = new OrderBook();
        var order = OrderStub.CreateBidOrder(12.6M, 100);

        var added = ob.AddOrder(order);
        Assert.Multiple(() =>
        {
            Assert.That(added, Is.True);
            Assert.That(ob.ValidateInternalState(), Is.True);
        });
    }

    [Test]
    public void CanRemoveOrder()
    {
        var ob = new OrderBook();
        var order = OrderStub.CreateBidOrder(12.6M, 100);

        var added = ob.AddOrder(order);
        Assert.That(added, Is.True);

        var removed = ob.RemoveOrder(order);
        Assert.Multiple(() =>
        {
            Assert.That(removed, Is.True);
            Assert.That(ob.ValidateInternalState(), Is.True);
        });
    }

    [Test]
    public void CanGetOrder()
    {
        var ob = new OrderBook();
        var order1 = OrderStub.CreateBidOrder(12.6M, 100);
        ob.AddOrder(order1);

        var o1 = ob.GetOrder(order1.OrderId);

        Assert.That(o1, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(o1.Price, Is.EqualTo(order1.Price));
            Assert.That(o1.Size, Is.EqualTo(order1.Size));
        });
    }

    [TestCase(100, 34, 66)]
    [TestCase(27, 15, 12)]
    [TestCase(27, 0, 27)]
    public void CanReduceOrder(long initSize, long reduceBy, long result)
    {
        var ob = new OrderBook();
        var order1 = OrderStub.CreateBidOrder(12.6M, initSize);

        ob.AddOrder(order1);
        ob.Reduce(order1, reduceBy);

        var o1 = ob.GetOrder(order1.OrderId);

        Assert.That(o1, Is.Not.Null);
        Assert.That(o1.Size, Is.EqualTo(result));
    }

    [TestCase(27, 34)]
    [TestCase(19, 19)]
    [TestCase(1, 2)]
    public void CanReduceMoreThanOrderSize(long initSize, long reduceBy)
    {
        var ob = new OrderBook();
        var order1 = OrderStub.CreateAskOrder(12.6M, initSize);

        ob.AddOrder(order1);
        ob.Reduce(order1, reduceBy);

        var o1 = ob.GetOrder(order1.OrderId);

        Assert.Multiple(() =>
        {
            Assert.That(o1, Is.Null);
            Assert.That(order1.Completed, Is.True);
        });
    }

    [Test]
    public void CannotReduceNegativeSize()
    {
        var ob = new OrderBook();
        var order1 = OrderStub.CreateBidOrder(12.6M, 13);

        ob.AddOrder(order1);

        Assert.Throws<ArgumentOutOfRangeException>(() => ob.Reduce(order1, -9));
    }

}
