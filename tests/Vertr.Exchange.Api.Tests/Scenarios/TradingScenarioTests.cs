using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Tests.Scenarios;

[TestFixture(Category = "Unit")]
public class TradingScenarioTests : ApiTestBase
{
    [Test]
    public async Task TradeBidAskMatching()
    {
        var makerUid = 100L;
        var takerUid = 102L;
        var symbol = 2;

        await AddUser(makerUid);
        await AddUser(takerUid);
        await AddSymbol(symbol);

        var bidRes = await PlaceGTCOrder(OrderAction.BID, makerUid, symbol, 23.45m, 34);
        var askRes = await PlaceGTCOrder(OrderAction.ASK, takerUid, symbol, 23.10m, 30);

        var taker = await GetTradeEvent(askRes.OrderId);
        Assert.That(taker, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(taker.TakeOrderCompleted, Is.True);
            Assert.That(taker.TotalVolume, Is.EqualTo(30));
            Assert.That(taker.TakerUid, Is.EqualTo(takerUid));
            Assert.That(taker.TakerAction, Is.EqualTo(OrderAction.ASK));
        });

        var maker = taker.Trades.First();

        Assert.That(maker, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(maker.MakerOrderCompleted, Is.False);
            Assert.That(maker.Price, Is.EqualTo(23.45m));
            Assert.That(maker.MakerUid, Is.EqualTo(makerUid));
            Assert.That(maker.MakerOrderId, Is.EqualTo(bidRes.OrderId));
            Assert.That(maker.Volume, Is.EqualTo(30));
        });

        //var makerRep = await GetUserReport(makerUid);
        //var takerRep = await GetUserReport(takerUid);
    }

}
