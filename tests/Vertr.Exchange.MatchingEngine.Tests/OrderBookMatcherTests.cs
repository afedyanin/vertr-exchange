using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.MatchingEngine.Tests.Stubs;

namespace Vertr.Exchange.MatchingEngine.Tests;

[TestFixture(Category = "Unit")]
public class OrderBookMatcherTests
{
    // TODO: Wrtite test
    [Test]
    public void CanMatchOrder()
    {
        var orderBook = new OrderBook();
        var bid = OrderStub.CreateBidOrder(45M, 2);
        orderBook.AddOrder(bid);

        var res = orderBook.TryMatchInstantly(OrderAction.ASK, 44M, 5, 1);

        Assert.That(res.Filled, Is.EqualTo(2));
    }
}
