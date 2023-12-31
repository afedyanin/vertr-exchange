using NUnit.Framework;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Accounts.Tests;

[TestFixture(Category = "Unit")]
public class EquityPositionTests
{
    [Test]
    public void LongCanUpdateWithAskClosePosition()
    {
        var pos = new EquityPosition(1L, 2);
        pos.Update(OrderAction.BID, 100, 10m);
        Console.WriteLine($"Bid 100x10$ (1000) => {pos}");
        pos.Update(OrderAction.ASK, 90, 100m);
        Console.WriteLine($"Ask 90x100$ (-9000) => {pos}");
        pos.Update(OrderAction.ASK, 5, 100m);
        Console.WriteLine($"Ask 5x100$ (-500) => {pos}");
        pos.Update(OrderAction.ASK, 5, 100m);
        Console.WriteLine($"Ask 5x100$ (-500) => {pos}");
    }

    [Test]
    public void LongCanUpdateWithAskOverlap()
    {
        var pos = new EquityPosition(1L, 2);
        pos.Update(OrderAction.BID, 100, 10m);
        Console.WriteLine($"Bid 100x10$ (1000) => {pos}");
        pos.Update(OrderAction.ASK, 90, 100m);
        Console.WriteLine($"Ask 90x100$ (-9000) => {pos}");
        pos.Update(OrderAction.ASK, 5, 100m);
        Console.WriteLine($"Ask 5x100$ (-500) => {pos}");
        pos.Update(OrderAction.ASK, 15, 20m);
        Console.WriteLine(pos);
    }
}
