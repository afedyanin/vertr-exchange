using NUnit.Framework;

namespace Vertr.Exchange.Accounts.Tests;

[TestFixture(Category = "Unit")]

public class PositionTests
{
    [Test]
    public void CanCreatePosition()
    {
        var pos = new Position(1, 2);

        Assert.That(pos, Is.Not.Null);
    }
}
