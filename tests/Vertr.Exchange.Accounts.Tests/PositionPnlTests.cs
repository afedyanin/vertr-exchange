using NUnit.Framework;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Accounts.Tests;

[TestFixture(Category = "Unit")]
public class PositionPnlTests
{
    [TestCase(10, 5.89)]
    [TestCase(15, 1.008)]
    [TestCase(35897, 0.00899)]
    public void SingleBid(long size, decimal price)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, size, price);
        var expectedPnl = size * price * (-1);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(size));
        });
    }

    [TestCase(10, 5.89)]
    [TestCase(15, 1.008)]
    [TestCase(35897, 0.00899)]
    public void SingleAsk(long size, decimal price)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, size, price);
        var expectedPnl = size * price * (-1);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(size));
        });
    }

    [Test]
    public void SequenseOfBids()
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 10, 3);
        var expectedPnl = 10 * 3 * (-1);
        var expectedOpenVol = 10;

        pos.Update(OrderAction.BID, 5, 4);
        expectedPnl += 5 * 4 * (-1);
        expectedOpenVol += 5;

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(expectedOpenVol));
        });
    }

    [Test]
    public void SequenseOfAsks()
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 10, 3);
        var expectedPnl = 10 * 3 * (-1);
        var expectedOpenVol = 10;

        pos.Update(OrderAction.ASK, 5, 4);
        expectedPnl += 5 * 4 * (-1);
        expectedOpenVol += 5;

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(expectedOpenVol));
        });
    }

    [Test]
    public void BidAskClosedPositionZeroPnl()
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, 7, 3.5m);
        pos.Update(OrderAction.ASK, 7, 3.5m);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(decimal.Zero));
            Assert.That(pos.OpenVolume, Is.EqualTo(decimal.Zero));
        });
    }

    [Test]
    public void AskBidClosedPositionZeroPnl()
    {
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.ASK, 7, 3.5m);
        pos.Update(OrderAction.BID, 7, 3.5m);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(decimal.Zero));
            Assert.That(pos.OpenVolume, Is.EqualTo(decimal.Zero));
        });
    }

    [Test]
    public void BidBidAskPartiallyClosedPosition()
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 7, 3.5m);
        var expectedOpenVol = 7;
        var expectedPnl = 7 * 3.5m * (-1);

        pos.Update(OrderAction.BID, 19, 8.5m);
        expectedOpenVol += 19;
        expectedPnl += 19 * 8.5m * (-1);

        pos.Update(OrderAction.ASK, 10, 5.7m);
        expectedOpenVol -= 10;
        expectedPnl += 10 * 5.7m;

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(expectedOpenVol));
        });
    }

    [Test]
    public void AskAskBidPartiallyClosedPosition()
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, 3.5m);
        var expectedOpenVol = 7;
        var expectedPnl = 7 * 3.5m * (-1);

        pos.Update(OrderAction.ASK, 19, 8.5m);
        expectedOpenVol += 19;
        expectedPnl += 19 * 8.5m * (-1);

        pos.Update(OrderAction.BID, 10, 5.7m);
        expectedOpenVol -= 10;
        expectedPnl += 10 * 5.7m;

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(expectedOpenVol));
        });
    }

    [TestCase(5.0, 5.0, 5.0)] // 7x5 + 3x5 - 10x5 = -(35 + 15) + 50 = 0
    [TestCase(5.0, 8.0, 9.0)] // 7x5 + 3x8 - 10x9 = -(35 + 24) + 90 = 31 
    [TestCase(5.0, 8.0, 6.0)] // 7x5 + 3x8 - 10x6 = -(35 + 24) + 60 = 1
    [TestCase(5.0, 8.0, 3.0)] // 7x5 + 3x8 - 10x3 = -(35 + 24) + 30 = -29
    public void BidBidAskExactClosedPosition(
        decimal bid1Price,
        decimal bid2Price,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 7, bid1Price);
        var expectedOpenVol = 7;
        var expectedPnl = 7 * bid1Price * (-1);

        pos.Update(OrderAction.BID, 3, bid2Price);
        expectedOpenVol += 3;
        expectedPnl += 3 * bid2Price * (-1);

        pos.Update(OrderAction.ASK, expectedOpenVol, askPrice);
        expectedPnl += expectedOpenVol * askPrice;

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(0));
        });

        Console.WriteLine($"{pos}");
    }

    [TestCase(5.0, 5.0, 5.0)] // 7x5 + 3x5 - 10x5 = (35 + 15) - 50 = 0
    [TestCase(5.0, 8.0, 9.0)] // 7x5 + 3x8 - 10x9 = (35 + 24) - 90 = -31 
    [TestCase(5.0, 8.0, 6.0)] // 7x5 + 3x8 - 10x6 = (35 + 24) - 60 = -1
    [TestCase(5.0, 8.0, 3.0)] // 7x5 + 3x8 - 10x3 = (35 + 24) - 30 = 29
    public void AskAskBidExactClosedPosition(
        decimal ask1Price,
        decimal ask2Price,
        decimal bidPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, ask1Price);
        var expectedOpenVol = 7;
        var expectedPnl = 7 * ask1Price ;

        pos.Update(OrderAction.ASK, 3, ask2Price);
        expectedOpenVol += 3;
        expectedPnl += 3 * ask2Price;

        pos.Update(OrderAction.BID, expectedOpenVol, bidPrice);
        expectedPnl += expectedOpenVol * bidPrice * (-1);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(0));
        });

        Console.WriteLine($"{pos}");
    }
}
