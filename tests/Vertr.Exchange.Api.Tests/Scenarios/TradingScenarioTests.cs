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

        var bidRes = await PlaceGTCOrder(OrderAction.BID, makerUid, symbol, 7m, 5);
        var askRes = await PlaceGTCOrder(OrderAction.ASK, takerUid, symbol, 3m, 2);

        var taker = await GetTradeEvent(askRes.OrderId);
        Assert.That(taker, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(taker.TakeOrderCompleted, Is.True);
            Assert.That(taker.TotalVolume, Is.EqualTo(2));
            Assert.That(taker.TakerUid, Is.EqualTo(takerUid));
            Assert.That(taker.TakerAction, Is.EqualTo(OrderAction.ASK));
        });

        var maker = taker.Trades.First();

        Assert.That(maker, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(maker.MakerOrderCompleted, Is.False);
            Assert.That(maker.Price, Is.EqualTo(7m));
            Assert.That(maker.MakerUid, Is.EqualTo(makerUid));
            Assert.That(maker.MakerOrderId, Is.EqualTo(bidRes.OrderId));
            Assert.That(maker.Volume, Is.EqualTo(2));
        });

        var makerRep = await GetUserReport(makerUid);

        Assert.That(makerRep, Is.Not.Null);
        Assert.That(makerRep.Positions, Is.Not.Empty);

        var makerPos = makerRep.Positions[symbol];
        Assert.That(makerPos, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(makerPos.Uid, Is.EqualTo(makerUid));
            Assert.That(makerPos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(makerPos.OpenVolume, Is.EqualTo(2));
            Assert.That(makerPos.RealizedPnL, Is.EqualTo(2 * 7m * (-1)));
        });

        var takerRep = await GetUserReport(takerUid);

        Assert.That(takerRep, Is.Not.Null);
        Assert.That(takerRep.Positions, Is.Not.Empty);

        var takerPos = takerRep.Positions[symbol];
        Assert.That(takerPos, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(takerPos.Uid, Is.EqualTo(takerUid));
            Assert.That(takerPos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(takerPos.OpenVolume, Is.EqualTo(2));
            Assert.That(takerPos.RealizedPnL, Is.EqualTo(2 * 7m * (-1)));
        });
    }

}
