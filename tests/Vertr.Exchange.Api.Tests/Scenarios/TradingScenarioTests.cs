using Vertr.Exchange.Common.Enums;

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
        await PlaceGTCOrder(OrderAction.BID, makerUid, symbol, 23.45m, 34, 23L);

        var res = await PlaceGTCOrder(OrderAction.ASK, takerUid, symbol, 23.10m, 30, 24L);

        var te = res.RootEvent;
        Assert.That(te, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(te.EventType, Is.EqualTo(EngineEventType.TRADE));
            Assert.That(te.Price, Is.EqualTo(23.45m));
            Assert.That(te.Size, Is.EqualTo(30));
            Assert.That(te.ActiveOrderCompleted, Is.True);
            Assert.That(te.MatchedOrderCompleted, Is.False);
            Assert.That(te.MatchedOrderId, Is.EqualTo(23L));
            Assert.That(te.MatchedOrderUid, Is.EqualTo(makerUid));
        });

        //var makerRep = await GetUserReport(makerUid);
        //var takerRep = await GetUserReport(takerUid);
    }

}
