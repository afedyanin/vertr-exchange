namespace Vertr.Terminal.Server.Tests;

[TestFixture(Category = "Unit")]
public class RecordTests
{
    public record History
    {
        public IList<HistoryItem> Items { get; init; }

        public Guid Id { get; init; }

        public int IdCount { get; init; }

        public decimal Amount { get; init; }

        public History(Guid id, decimal amount)
        {
            Id = id;
            Amount = amount;
            IdCount = 1;

            Items = new List<HistoryItem>
            {
                new HistoryItem(id, Amount)
            };
        }

        public HistoryItem[] GetItems => Items.ToArray();

        public History Add(History item)
        {
            var amount = Amount + item.Amount;
            var idCount = IdCount + 1;

            Items.Add(new HistoryItem(item.Id, item.Amount));

            return this with
            {
                Amount = amount,
                IdCount = idCount,
            };
        }
    }

    public record HistoryItem(Guid Id, decimal Price);


    [Test]
    public void CanCompareHistoryItems()
    {
        var h1 = new History(Guid.NewGuid(), 100);
        var h2 = h1.Add(new History(Guid.NewGuid(), 50));

        var items1 = h1.Items;
        var items2 = h2.Items;

        Assert.That(items1, Is.EqualTo(items2));
        Assert.That(items1, Has.Count.EqualTo(items2.Count));
    }

    [Test]
    public void CanModifyHistoryItems()
    {
        var h1 = new History(Guid.NewGuid(), 100);
        var h2 = h1.Add(new History(Guid.NewGuid(), 50));

        var items1 = h1.Items;
        var items2 = h2.Items;

        items1.Add(new HistoryItem(Guid.NewGuid(), 80));

        Assert.That(items1, Is.EqualTo(items2));
        Assert.That(items1, Has.Count.EqualTo(items2.Count));
    }
}
