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

        Console.WriteLine($"{pos}");
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

        Console.WriteLine($"{pos}");
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

    [TestCase(3.5, 3.5)] // -7x3.5 + 7x3.5 = 0
    [TestCase(5.0, 8.0)] // -7x5.0 + 7x8.0 = -35 + 56 = 21
    [TestCase(5.0, 3.0)] // -7x5.0 + 7x2.0 = -35 + 21 = -14
    public void BidAskClosed(
        decimal bidPrice,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 7, bidPrice);
        var expectedPnl = 7 * bidPrice * (-1);

        pos.Update(OrderAction.ASK, 7, askPrice);
        expectedPnl += 7 * askPrice;

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(0L));
        });

        Console.WriteLine($"{pos}");
    }

    [TestCase(5, 5)] // -7x5 + 7x5 - 10x5 = -50
    [TestCase(5, 8)] // -7x5 + 7x8 - 10x8 = -35 + 56 - 80 = 21 - 80 = -59
    [TestCase(5, 3)] // -7x5 + 7x3 - 10x3 = -35 + 21 - 30 = -14 - 30 = -44
    public void BidAskOverlapped(
        decimal bidPrice,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 7, bidPrice);
        var expectedPnl = 7 * bidPrice * (-1);
        Console.WriteLine($"{pos}");

        pos.Update(OrderAction.ASK, 7, askPrice);
        expectedPnl += 7 * askPrice; // close position
        Console.WriteLine($"{pos}");

        pos.Update(OrderAction.ASK, 10, askPrice);
        expectedPnl += 10 * askPrice * (-1); // open short
        Console.WriteLine($"{pos}");

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(17 - 7));
        });
    }

    [TestCase(3.5, 3.5)] // 7x3.5 - 7x3.5 = 0
    [TestCase(5.0, 8.0)] // 7x8.0 - 7x5.0 = 56 - 35 = 21
    [TestCase(5.0, 3.0)] // 7x3.0 - 7x5.0 = 21 - 35 = -14
    public void AskBidClosed(
        decimal bidPrice,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, askPrice);
        var expectedPnl = 7 * askPrice;

        pos.Update(OrderAction.BID, 7, bidPrice);
        expectedPnl += 7 * bidPrice * (-1);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(0L));
        });

        Console.WriteLine($"{pos}");
    }

    [TestCase(3.5, 3.5)] // (7x3.5 - 7x3.5) - 10x3,5 = 24.5 - 24.5 - 35 = -35
    [TestCase(5.0, 8.0)] // (7x8.0 - 7x5.0) - 10x8.0 = 56 - 35 - 80 = -99
    [TestCase(5.0, 3.0)] // (7x3.0 - 7x5.0) - 10x8.0 = 21 - 35 - 80 = -94
    public void AskBidOverlapped(
        decimal bidPrice,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, askPrice);
        var expectedPnl = 7 * askPrice;

        pos.Update(OrderAction.BID, 17, bidPrice);
        expectedPnl += 7 * bidPrice * (-1); // close
        expectedPnl += 10 * bidPrice * (-1); // open long

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(17 - 7));
        });

        Console.WriteLine($"{pos}");
    }


    [Test]
    public void BidBidAskPartiallyClosed()
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
    public void AskAskBidPartiallyClosed()
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

    [TestCase(5.0, 5.0, 5.0)] // -(7x5 + 3x5) + 10x5 = -(35 + 15) + 50 = 0
    [TestCase(5.0, 8.0, 9.0)] // -(7x5 + 3x8) + 10x9 = -(35 + 24) + 90 = 31 
    [TestCase(5.0, 8.0, 6.0)] // -(7x5 + 3x8) + 10x6 = -(35 + 24) + 60 = 1
    [TestCase(5.0, 8.0, 3.0)] // -(7x5 + 3x8) + 10x3 = -(35 + 24) + 30 = -29
    public void BidBidAskClosed(
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
    public void AskAskBidClosed(
        decimal ask1Price,
        decimal ask2Price,
        decimal bidPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, ask1Price);
        var expectedOpenVol = 7;
        var expectedPnl = 7 * ask1Price;

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

    [TestCase(5.0, 5.0, 5.0)] // -(7x5 + 3x5) + 10x5 - 5x5 = -(35 + 15) + 50 - 25 = 0 - 25 = -25
    [TestCase(5.0, 8.0, 9.0)] // -(7x5 + 3x8) + 10x9 - 5x9 = -(35 + 24) + 90 - 45 = 31 - 45 = -14 
    [TestCase(5.0, 8.0, 6.0)] // -(7x5 + 3x8) + 10x6 - 5x6 = -(35 + 24) + 60 - 30 = 1 - 30 = -29
    [TestCase(5.0, 8.0, 3.0)] // -(7x5 + 3x8) + 10x3 - 5x3 = -(35 + 24) + 30 - 15 = -29 - 15 = -44
    public void BidBidAskOverlapped(
        decimal bid1Price,
        decimal bid2Price,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 7, bid1Price);
        var expectedPnl = 7 * bid1Price * (-1);

        pos.Update(OrderAction.BID, 3, bid2Price);
        expectedPnl += 3 * bid2Price * (-1);

        pos.Update(OrderAction.ASK, 15, askPrice);
        expectedPnl += 10 * askPrice;
        expectedPnl += 5 * askPrice * (-1);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(5));
        });

        Console.WriteLine($"{pos}");
    }

    [TestCase(5.0, 5.0, 5.0)] // 7x5 + 3x5 - 10x5 - 5x5 = (35 + 15) - 50 - 25 = 0 - 25 = -25
    [TestCase(5.0, 8.0, 9.0)] // 7x5 + 3x8 - 10x9 - 5x9 = (35 + 24) - 90 - 45 = -31 - 45 = -76
    [TestCase(5.0, 8.0, 6.0)] // 7x5 + 3x8 - 10x6 - 5x6 = (35 + 24) - 60 - 30 = -1 - 30 = -31
    [TestCase(5.0, 8.0, 3.0)] // 7x5 + 3x8 - 10x3 - 5x3 = (35 + 24) - 30 - 15 = 29 - 15 = 14
    public void AskAskBidOverlapped(
        decimal ask1Price,
        decimal ask2Price,
        decimal bidPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, ask1Price);
        var expectedPnl = 7 * ask1Price;

        pos.Update(OrderAction.ASK, 3, ask2Price);
        expectedPnl += 3 * ask2Price;

        pos.Update(OrderAction.BID, 15, bidPrice);
        expectedPnl += 10 * bidPrice * (-1);
        expectedPnl += 5 * bidPrice * (-1);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(5));
        });

        Console.WriteLine($"{pos}");
    }

    [TestCase(5.0, 5.0, 5.0)] // -7x5 + (7x5 - 2x5) + 2x5 = -35 + (35 - 10) + 10 = -35 + 25 + 10 = -10 + 10 = 0
    [TestCase(5.0, 8.0, 9.0)] // -7x5 + (7x9 - 2x9) + 2x8 = -35 + (63 - 18) + 16 = -35 + 45 + 16 = 10 + 16 = 26  
    [TestCase(5.0, 8.0, 6.0)]
    [TestCase(5.0, 8.0, 3.0)]
    public void BidAskOverlappedBidClosed(
        decimal bid1Price,
        decimal bid2Price,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 7, bid1Price);
        var expectedPnl = 7 * bid1Price * (-1);
        Console.WriteLine($"Pnl={expectedPnl} Pos={pos}");

        pos.Update(OrderAction.ASK, 9, askPrice);
        expectedPnl += 7 * askPrice;
        var fixedPnl = expectedPnl;
        expectedPnl = 2 * askPrice * (-1);
        Console.WriteLine($"Pnl={fixedPnl + expectedPnl} Pos={pos}");

        pos.Update(OrderAction.BID, 2, bid2Price);
        expectedPnl += 2 * bid2Price;
        expectedPnl *= -1; // close short
        fixedPnl += expectedPnl;
        Console.WriteLine($"Pnl={fixedPnl} Pos={pos}");

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.OpenVolume, Is.EqualTo(0));
            Assert.That(pos.RealizedPnL, Is.EqualTo(fixedPnl));
        });
    }

    [TestCase(5.0, 5.0, 5.0)]
    [TestCase(5.0, 8.0, 9.0)]
    [TestCase(5.0, 8.0, 6.0)]
    [TestCase(5.0, 8.0, 3.0)]
    public void BidAskOverlappedBidOverlapped(
        decimal bid1Price,
        decimal bid2Price,
        decimal askPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.BID, 7, bid1Price);
        var expectedPnl = 7 * bid1Price * (-1);
        Console.WriteLine($"Pnl={expectedPnl} Pos={pos}");

        pos.Update(OrderAction.ASK, 9, askPrice);
        expectedPnl += 7 * askPrice;
        var fixedPnl = expectedPnl;
        expectedPnl = 2 * askPrice * (-1);
        Console.WriteLine($"Pnl={fixedPnl + expectedPnl} Pos={pos}");

        pos.Update(OrderAction.BID, 5, bid2Price);
        expectedPnl += 2 * bid2Price;
        expectedPnl *= -1; // close short
        fixedPnl += expectedPnl;
        expectedPnl = 3 * bid2Price * (-1); // open long
        Console.WriteLine($"Pnl={fixedPnl + expectedPnl} Pos={pos}");

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_LONG));
            Assert.That(pos.OpenVolume, Is.EqualTo(3));
            Assert.That(pos.RealizedPnL, Is.EqualTo(fixedPnl + expectedPnl));
        });
    }

    [TestCase(5.0, 5.0, 5.0)]
    [TestCase(5.0, 8.0, 9.0)]
    [TestCase(5.0, 8.0, 6.0)]
    [TestCase(5.0, 8.0, 3.0)]
    public void AskBidOverlappedAskClosed(
        decimal ask1Price,
        decimal ask2Price,
        decimal bidPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, ask1Price);
        var expectedPnl = 7 * ask1Price * (-1);
        Console.WriteLine($"Pnl={expectedPnl} Pos={pos}");

        pos.Update(OrderAction.BID, 9, bidPrice);
        expectedPnl += 7 * bidPrice;
        var fixedPnl = expectedPnl;
        expectedPnl = 2 * bidPrice * (-1);
        Console.WriteLine($"Pnl={fixedPnl + expectedPnl} Pos={pos}");

        pos.Update(OrderAction.ASK, 2, ask2Price);
        expectedPnl += 2 * ask2Price;
        fixedPnl += expectedPnl;
        Console.WriteLine($"Pnl={fixedPnl} Pos={pos}");

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.EMPTY));
            Assert.That(pos.OpenVolume, Is.EqualTo(0));
            Assert.That(pos.RealizedPnL, Is.EqualTo(fixedPnl));
        });
    }

    [TestCase(5.0, 5.0, 5.0)]
    [TestCase(5.0, 8.0, 9.0)]
    [TestCase(5.0, 8.0, 6.0)]
    [TestCase(5.0, 8.0, 3.0)]
    public void AskBidOverlappedAskOverlapped(
        decimal ask1Price,
        decimal ask2Price,
        decimal bidPrice)
    {
        var pos = new Position(1L, 2);

        pos.Update(OrderAction.ASK, 7, ask1Price);
        var expectedPnl = 7 * ask1Price * (-1);
        Console.WriteLine($"Pnl={expectedPnl} Pos={pos}");

        pos.Update(OrderAction.BID, 9, bidPrice);
        expectedPnl += 7 * bidPrice;
        var fixedPnl = expectedPnl;
        expectedPnl = 2 * bidPrice * (-1);
        Console.WriteLine($"Pnl={fixedPnl + expectedPnl} Pos={pos}");

        pos.Update(OrderAction.ASK, 5, ask2Price);
        expectedPnl += 2 * ask2Price;
        fixedPnl += expectedPnl;
        expectedPnl = 3 * ask2Price * (-1); // open short
        Console.WriteLine($"Pnl={fixedPnl + expectedPnl} Pos={pos}");

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(pos.OpenVolume, Is.EqualTo(3));
            Assert.That(pos.RealizedPnL, Is.EqualTo(fixedPnl + expectedPnl));
        });
    }

}
