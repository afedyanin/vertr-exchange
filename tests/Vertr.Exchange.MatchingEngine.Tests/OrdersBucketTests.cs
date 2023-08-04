using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests;

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
            Assert.That(ob.OrdersCount, Is.EqualTo(0));
        });
    }

    [Test]
    public void CanPutOrder()
    {
        var price = 78.23M;
        var ob = new OrdersBucket(price);

        var order = OrderStub.CreateBidOrder(price, 100);
        ob.Put(order);

        Assert.That(ob, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(ob.Price, Is.EqualTo(price));
            Assert.That(ob.IsValid(), Is.EqualTo(true));
            Assert.That(ob.OrdersCount, Is.EqualTo(1));
        });
    }
}
