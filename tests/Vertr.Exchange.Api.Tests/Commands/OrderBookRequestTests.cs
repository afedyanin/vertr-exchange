using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Tests.Commands;

[TestFixture(Category = "Unit")]
public class OrderBookRequestTests : CommandTestBase
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

        var obr = new OrderBookRequest(23L, DateTime.UtcNow, symbol, 100);
        res = await Api.SendAsync(obr);

        Assert.Multiple(() =>
        {
            Assert.That(res.ResultCode, Is.EqualTo(CommandResultCode.SUCCESS));
            Assert.That(res.MarketData, Is.Not.Null);
        });

        Assert.Multiple(() =>
        {
            Assert.That(res.MarketData.AskSize, Is.EqualTo(0));
            Assert.That(res.MarketData.AskOrders, Is.Empty);
            Assert.That(res.MarketData.AskPrices, Is.Empty);
            Assert.That(res.MarketData.AskVolumes, Is.Empty);
            Assert.That(res.MarketData.BidSize, Is.EqualTo(1));
            Assert.That(res.MarketData.BidOrders, Has.Length.EqualTo(1));
            Assert.That(res.MarketData.BidPrices, Has.Length.EqualTo(1));
            Assert.That(res.MarketData.BidVolumes, Has.Length.EqualTo(1));
            Assert.That(res.MarketData.BidOrders[0], Is.EqualTo(1));
            Assert.That(res.MarketData.BidPrices[0], Is.EqualTo(23.45m));
            Assert.That(res.MarketData.BidVolumes[0], Is.EqualTo(34));
        });
    }
}
