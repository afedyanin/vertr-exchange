using NUnit.Framework;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Accounts.Tests;

[TestFixture(Category = "Unit")]
public class PositionPnlTests
{
    [TestCase(10, 5.89)]
    [TestCase(15, 1.008)]
    [TestCase(35897, 0.00899)]
    public void CanOpenSingleBid(long size, decimal price)
    {
        var expectedPnl = size * price * (-1);
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.BID, size, price);

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
    public void CanOpenSingleAsk(long size, decimal price)
    {
        var expectedPnl = size * price * (-1);
        var pos = new Position(1L, 2);
        pos.Update(OrderAction.ASK, size, price);

        Assert.Multiple(() =>
        {
            Assert.That(pos.Direction, Is.EqualTo(PositionDirection.DIR_SHORT));
            Assert.That(pos.RealizedPnL, Is.EqualTo(expectedPnl));
            Assert.That(pos.OpenVolume, Is.EqualTo(size));
        });
    }
}
