using Vertr.Terminal.DataAccess.InMemory.Repositories;

namespace Vertr.Terminal.DataAccess.InMemory.Tests.Repositories;

[TestFixture(Category = "Integration")]
public class MarketDataRepositoryTests
{
    [Test]
    public async Task CanUpdateMarketData()
    {
        var repo = new MarketDataRepository();
        var symbolId = 100;
        var price = 45.5m;
        var dt = new DateTime(2024, 01, 25, 18, 12, 57);

        var res = await repo.Update(symbolId, dt, price);

        Assert.That(res, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(res.SymbolId, Is.EqualTo(symbolId), "SymbolId");
            Assert.That(res.Price, Is.EqualTo(price), "Price");
            Assert.That(res.DayHigh, Is.EqualTo(price), "DayHigh");
            Assert.That(res.DayLow, Is.EqualTo(price), "DayLow");
            Assert.That(res.DayOpen, Is.EqualTo(price), "DayOpen");
            Assert.That(res.PercentChange, Is.EqualTo(0), "PercentChange");
            Assert.That(res.Change, Is.EqualTo(0), "Change");
            Assert.That(res.LastChange, Is.EqualTo(price), "LastChange");
            Assert.That(res.TimeStamp, Is.EqualTo(dt), "TimeStamp");
        });
    }

    [TestCase(10, 20)]
    [TestCase(30, 15)]
    [TestCase(17, 17)]
    public async Task CanUpdateMarketDataTwice(decimal p1, decimal p2)
    {
        var symbolId = 100;
        var repo = new MarketDataRepository();
        var dt1 = new DateTime(2024, 01, 25, 18, 12, 57);
        var dt2 = new DateTime(2024, 01, 25, 18, 15, 57);

        var res = await repo.Update(symbolId, dt1, p1);
        res = await repo.Update(symbolId, dt2, p2);

        Assert.That(res, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(res.SymbolId, Is.EqualTo(symbolId));
            Assert.That(res.Price, Is.EqualTo(p2));
            Assert.That(res.DayHigh, Is.EqualTo(Math.Max(p1, p2)));
            Assert.That(res.DayLow, Is.EqualTo(Math.Min(p1, p2)));
            Assert.That(res.DayOpen, Is.EqualTo(p1));
            Assert.That(Math.Round(res.PercentChange, 4), Is.EqualTo(Math.Round((p2 - p1) / p2, 4)));
            Assert.That(res.Change, Is.EqualTo(p2 - p1));
            Assert.That(res.LastChange, Is.EqualTo(p2 == p1 ? p2 : p2 - p1));
            Assert.That(res.TimeStamp, Is.EqualTo(p2 == p1 ? dt1 : dt2));
        });
    }

    [TestCase(17, 17, 17)]
    [TestCase(10, 20, 30)]
    [TestCase(30, 20, 10)]
    [TestCase(10, 10, 30)]
    [TestCase(30, 30, 10)]
    [TestCase(10, 30, 30)]
    [TestCase(30, 20, 20)]
    [TestCase(10, 30, 20)]
    [TestCase(30, 10, 20)]
    public async Task CanUpdateMarketDataTriple(decimal p1, decimal p2, decimal p3)
    {
        var symbolId = 100;
        var repo = new MarketDataRepository();
        var dt1 = new DateTime(2024, 01, 25, 18, 12, 57);
        var dt2 = new DateTime(2024, 01, 25, 18, 15, 57);
        var dt3 = new DateTime(2024, 01, 25, 18, 15, 59);

        var res = await repo.Update(symbolId, dt1, p1);
        res = await repo.Update(symbolId, dt2, p2);
        res = await repo.Update(symbolId, dt3, p3);

        var prices = new decimal[] { p1, p2, p3 };

        Assert.That(res, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(res.SymbolId, Is.EqualTo(symbolId));
            Assert.That(res.Price, Is.EqualTo(p3));

            Assert.That(res.DayHigh, Is.EqualTo(prices.Max()));
            Assert.That(res.DayLow, Is.EqualTo(prices.Min()));
            Assert.That(res.DayOpen, Is.EqualTo(p1));
            Assert.That(Math.Round(res.PercentChange, 4), Is.EqualTo(Math.Round((p3 - p1) / p3, 4)));
            Assert.That(res.Change, Is.EqualTo(p3 - p1));
            Assert.That(res.LastChange, Is.EqualTo(p3 == p2 ? p2 == p1 ? p2 : p2 - p1 : p3 - p2));
            Assert.That(res.TimeStamp, Is.EqualTo(p3 == p2 ? p2 == p1 ? dt1 : dt2 : dt3));
        });
    }
}
