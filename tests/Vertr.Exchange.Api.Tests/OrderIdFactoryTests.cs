using Vertr.Exchange.Api.Factories;

namespace Vertr.Exchange.Api.Tests;

[TestFixture(Category = "Unit")]
public class OrderIdFactoryTests
{
    [Test]
    public void CanGetIncremetedIds()
    {
        var factory = new OrderIdFactorty();
        var id1 = factory.NextId;
        var id2 = factory.NextId;
        var id3 = factory.NextId;
        Assert.Multiple(() =>
        {
            Assert.That(id1, Is.GreaterThan(0L));
            Assert.That(id2, Is.GreaterThan(id1));
            Assert.That(id3, Is.GreaterThan(id2));
        });
    }

    [Test]
    public void CanGetIdsInParallel()
    {
        var items = 1_000_000;
        var factory = new OrderIdFactorty();
        var hs = new long[items];

        var options = new ParallelOptions()
        {
            MaxDegreeOfParallelism = 16
        };

        Parallel.For(0, items, options, index =>
        {
            hs[index] = factory.NextId;
        });

        // var ids = string.Join(",", hs);
        // Console.WriteLine(ids);

        var zeroes = hs.Any(i => i == 0L);
        Assert.That(zeroes, Is.False);
        var distinct = hs.Distinct().Count();
        Assert.That(distinct, Is.EqualTo(items));

        var next = factory.NextId;
        Assert.That(next, Is.EqualTo(items + 1));
    }
}
