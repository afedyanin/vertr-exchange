using NUnit.Framework;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Accounts.Tests;

[TestFixture(Category = "Unit")]

public class PositionTests
{
    [Test]
    public void CanCreatePosition()
    {
        var pos = new Position(1L, 2);

        Assert.That(pos, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Uid, Is.EqualTo(1L));
            Assert.That(pos.Symbol, Is.EqualTo(2));
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(decimal.Zero));
            Assert.That(pos.OpenVolume, Is.EqualTo(decimal.Zero));
        });
    }

    [TestCase(10, 5.89)]
    [TestCase(15, 1.008)]
    [TestCase(35897, 0.00899)]
    public void CanOpenBid(long size, decimal price)
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, size, price);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(decimal.Zero));
            Assert.That(pos.OpenVolume, Is.EqualTo(size));
        });
    }

    [TestCase(0, 1.99)]
    public void CanOpenZeroSizeBid(long size, decimal price)
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, size, price);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(decimal.Zero));
            Assert.That(pos.OpenVolume, Is.EqualTo(size));
        });
    }

    [TestCase(10, 0)]
    public void CanOpenZeroPriceBid(long size, decimal price)
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, size, price);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(decimal.Zero));
            Assert.That(pos.OpenVolume, Is.EqualTo(decimal.Zero));
        });
    }

    [Test]
    public void LongCanUpdateWithAsk()
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, 100, 10);

        pos.Update(OrderAction.ASK, 90, 100);
        Console.WriteLine(pos);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(0));
            Assert.That(pos.OpenVolume, Is.EqualTo(10));
        });

        pos.Update(OrderAction.ASK, 10, 100);
        Console.WriteLine(pos);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(9000));
            Assert.That(pos.OpenVolume, Is.EqualTo(0));
        });
    }

    [Test]
    public void LongCanUpdateWithAskOverlap()
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, 100, 10);
        pos.Update(OrderAction.ASK, 90, 100);
        Console.WriteLine(pos);
        pos.Update(OrderAction.ASK, 5, 100);
        Console.WriteLine(pos);
        pos.Update(OrderAction.ASK, 15, 20);
        Console.WriteLine(pos);
    }

    [Test]
    public void LongCanUpdateWithBid()
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, 100, 10);
        pos.Update(OrderAction.BID, 200, 20);
        Console.WriteLine(pos);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(0));
            Assert.That(pos.OpenVolume, Is.EqualTo(300));
        });
    }

    [Test]
    public void LongCanGetUnrealizedPnl()
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, 100, 10);
        Console.WriteLine(pos);

        pos.Update(OrderAction.ASK, 90, 100);
        Console.WriteLine(pos);

        var pnl = pos.GetUnrealizedPnL(20);
        Console.WriteLine($"pnl={pnl}");

        Assert.That(pnl, Is.EqualTo(8200));
    }

    [Test]
    public void ShortCanGetUnrealizedPnl()
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.ASK, 100, 10);
        Console.WriteLine(pos);

        var pnl = pos.GetUnrealizedPnL(20);
        Console.WriteLine($"pnl={pnl}");

        Assert.That(pnl, Is.EqualTo(-1000));
    }
}
