using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Tests.Scenarios;

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

        var bidRes = await PlaceGTCOrder(OrderAction.BID, makerUid, symbol, 3m, 2);
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
            Assert.That(maker.MakerOrderCompleted, Is.True);
            Assert.That(maker.Price, Is.EqualTo(3m));
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
            Assert.That(makerPos.PnL, Is.EqualTo(2 * 3m * (-1)));
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
            Assert.That(takerPos.PnL, Is.EqualTo(2 * 3m * (-1)));
        });
    }

    [Test]
    public async Task TradesBidAskClosePositions()
    {
        var makerUid = 100L;
        var takerUid = 102L;
        var symbol = 2;

        await AddUser(makerUid);
        await AddUser(takerUid);
        await AddSymbol(symbol);

        var bid1Res = await PlaceGTCOrder(OrderAction.BID, makerUid, symbol, 3m, 2);
        var ask1Res = await PlaceGTCOrder(OrderAction.ASK, takerUid, symbol, 3m, 2);

        var bid2Res = await PlaceGTCOrder(OrderAction.ASK, makerUid, symbol, 3m, 2);
        var ask2Res = await PlaceGTCOrder(OrderAction.BID, takerUid, symbol, 3m, 2);

        var makerRep = await GetUserReport(makerUid);

        Assert.That(makerRep, Is.Not.Null);
        // TODO: Move pnl to account and remove closed position
        Assert.That(makerRep.Positions, Is.Not.Empty);

        var takerRep = await GetUserReport(takerUid);

        Assert.That(takerRep, Is.Not.Null);
        // TODO: Move pnl to account and remove closed position
        Assert.That(takerRep.Positions, Is.Not.Empty);
    }

    [Test]
    public async Task TradesBidAskClosePositionsWithPnl()
    {
        var makerUid = 100L;
        var takerUid = 102L;
        var symbol = 2;

        await AddUser(makerUid);
        await AddUser(takerUid);
        await AddSymbol(symbol);

        var bid1Res = await PlaceGTCOrder(OrderAction.BID, makerUid, symbol, 3m, 3); // -9
        var ask1Res = await PlaceGTCOrder(OrderAction.ASK, takerUid, symbol, 3m, 3); // -9

        var bid2Res = await PlaceGTCOrder(OrderAction.ASK, makerUid, symbol, 5m, 4); // -9 + 15 - 5 = 6 - 5 = 1
        var ask2Res = await PlaceGTCOrder(OrderAction.BID, takerUid, symbol, 5m, 4); // +9 - 15 - 5 = -6 - 5 = -11

        var makerRep = await GetUserReport(makerUid);

        Assert.That(makerRep, Is.Not.Null);
        Assert.That(makerRep.Positions, Is.Not.Empty);

        var makerPos = makerRep.Positions[symbol];
        Assert.That(makerPos, Is.Not.Null);

        Console.WriteLine($"Maker: Pnl={1} {makerPos}");

        Assert.Multiple(() =>
        {
            Assert.That(makerPos.Uid, Is.EqualTo(makerUid));
            Assert.That(makerPos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(makerPos.OpenVolume, Is.EqualTo(1));
            Assert.That(makerPos.PnL, Is.EqualTo(1));
        });

        var takerRep = await GetUserReport(takerUid);

        Assert.That(takerRep, Is.Not.Null);
        Assert.That(takerRep.Positions, Is.Not.Empty);

        var takerPos = takerRep.Positions[symbol];
        Assert.That(takerPos, Is.Not.Null);

        Console.WriteLine($"Taker: Pnl={-11} {takerPos}");

        Assert.Multiple(() =>
        {
            Assert.That(takerPos.Uid, Is.EqualTo(takerUid));
            Assert.That(takerPos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(takerPos.OpenVolume, Is.EqualTo(1));
            Assert.That(takerPos.PnL, Is.EqualTo(-11));
        });
    }
}
