using Vertr.Exchange.Common;
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

        var orderCommand = new OrderCommand
        {
        };

        var res = orderBook.TryMatchInstantly(orderCommand);

        Assert.That(res, Is.EqualTo(2));
    }
}
