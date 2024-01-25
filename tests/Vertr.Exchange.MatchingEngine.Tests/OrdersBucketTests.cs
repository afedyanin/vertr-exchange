using Vertr.Exchange.Domain.MatchingEngine.OrderBooks;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.Domain.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrdersBucketTests
{
    [Test]
    public void CanCreateBucket()
    {
        var price = 78.2364654655M;

        var ob = new OrdersBucket(price);

        Assert.That(ob, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(ob.Price, Is.EqualTo(price));
            Assert.That(ob.IsValid(), Is.EqualTo(true));
            Assert.That(ob.OrdersCount, Is.EqualTo(0L));
            Assert.That(ob.TotalVolume, Is.EqualTo(0L));
        });
    }

    [Test]
    public void CannotCreateBucketWithNegativePrice()
    {
        var price = -893.21001M;
        Assert.Throws<ArgumentOutOfRangeException>(() => new OrdersBucket(price));
    }

    [TestCase(100, 0)]
    [TestCase(100, 56)]
    [TestCase(100, 100)]
    public void CanPutOrder(long size, long filled)
    {
        var price = 78.23M;
        var ob = new OrdersBucket(price);
        var order = OrderStub.CreateBidOrder(price, size, filled);

        ob.Put(order);

        Assert.That(ob, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(ob.Price, Is.EqualTo(price));
            Assert.That(ob.IsValid(), Is.EqualTo(true));
            Assert.That(ob.OrdersCount, Is.EqualTo(1));
            Assert.That(ob.TotalVolume, Is.EqualTo(order.Remaining));
        });
    }

    [Test]
    public void CannotPutOrderWithDifPrice()
    {
        var price = 78.23M;
        var ob = new OrdersBucket(price);
        var order = OrderStub.CreateBidOrder(price + 0.000001M, 100);

        Assert.Throws<InvalidOperationException>(() => ob.Put(order));
    }

    [Test]
    public void CanFindOrder()
    {
        var price = 78213.234567M;
        var ob = new OrdersBucket(price);
        var order = OrderStub.CreateBidOrder(price, 100);
        ob.Put(order);

        var found = ob.FindOrder(order.OrderId);

        Assert.That(found, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(found.Price, Is.EqualTo(price));
            Assert.That(found.Size, Is.EqualTo(order.Size));
            Assert.That(found.Timestamp, Is.EqualTo(order.Timestamp));
        });
    }

    [Test]
    public void CanRemoveOrder()
    {
        var price = 0.000007M;
        var ob = new OrdersBucket(price);
        var order = OrderStub.CreateBidOrder(price, 46554);
        ob.Put(order);

        var removed = ob.Remove(order);

        Assert.Multiple(() =>
        {
            Assert.That(removed, Is.True);
            Assert.That(ob.TotalVolume, Is.EqualTo(0L));
        });
    }

    [TestCase(356, 150, 100)]
    [TestCase(17, 0, 10)]
    [TestCase(97, 0, 0)]
    [TestCase(37, 17, 2)]
    [TestCase(37, 18, 0)]
    public void CanReduceSize(long size, long filled, long toReduce)
    {
        var price = 0.000007M;
        var ob = new OrdersBucket(price);
        var order = OrderStub.CreateBidOrder(price, size, filled);
        ob.Put(order);

        ob.ReduceSize(toReduce);

        Assert.That(ob.TotalVolume, Is.EqualTo(size - (filled + toReduce)));
    }

    [Test]
    public void ReduceNegativeThrows()
    {
        var price = 79887.0099M;
        var ob = new OrdersBucket(price);
        var order = OrderStub.CreateBidOrder(price, 12);
        ob.Put(order);

        Assert.Throws<ArgumentOutOfRangeException>(() => ob.ReduceSize(-99));
    }

    [TestCase(356, 150, 1000)]
    [TestCase(17, 0, 18)]
    [TestCase(97, 0, 108)]
    [TestCase(37, 17, 21)]
    [TestCase(37, 8, 30)]
    public void CannotReduceInvalidValues(long size, long filled, long toReduce)
    {
        var price = 0.000007M;
        var ob = new OrdersBucket(price);
        var order = OrderStub.CreateBidOrder(price, size, filled);
        ob.Put(order);

        Assert.Throws<InvalidOperationException>(() => ob.ReduceSize(toReduce));
    }
}
