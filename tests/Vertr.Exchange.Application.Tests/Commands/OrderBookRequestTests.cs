using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Tests.Commands;

[TestFixture(Category = "Unit")]
public class OrderBookRequestTests : ApiTestBase
{
    [Test]
    public async Task CanGetOrderBook()
    {
        var uid = 100L;
        await AddUser(uid);

        var symbol = 2;
        await AddSymbol(symbol);

        var res = await PlaceGTCOrder(OrderAction.BID, uid, symbol, 23.45m, 34);
        var orderId = res.OrderId;

        var book = await GetOrderBook(symbol);
        Assert.That(book, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(book.Asks, Is.Empty);
            Assert.That(book.Bids.Count, Is.EqualTo(1));
        });

        var bid = book.Bids.First();

        Assert.Multiple(() =>
        {
            Assert.That(bid.Price, Is.EqualTo(23.45m));
            Assert.That(bid.Orders, Is.EqualTo(1));
            Assert.That(bid.Volume, Is.EqualTo(34));
        });
    }
}
