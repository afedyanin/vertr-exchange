using Vertr.OrderMatching.Core.Priority;

namespace Vertr.OrderMatching.Core.Tests
{
    public class PriceTimePriorityTests
    {
        private readonly DateTime _before = new(2023, 05, 17, 20, 00, 00, DateTimeKind.Utc);
        private readonly DateTime _after = new(2023, 05, 17, 20, 10, 00, DateTimeKind.Utc);

        private readonly decimal _more = 15.56m; 
        private readonly decimal _less = 12.48m; 

        [Test]
        public void LowerPriceFirstWithAskComparer()
        {
            var low = new PriceTimePriority(_less, _after);
            var high = new PriceTimePriority(_more, _after);
            var comparer = new PriceTimePriorityAskComparer();

            var res = comparer.Compare(low, high);

            Assert.That(res, Is.LessThan(0));
        }

        [Test]
        public void BeforeTimeFirstWithAskComparer()
        {
            var low = new PriceTimePriority(_less, _before);
            var high = new PriceTimePriority(_less, _after);
            var comparer = new PriceTimePriorityAskComparer();

            var res = comparer.Compare(low, high);

            Assert.That(res, Is.LessThan(0));
        }

        [Test]
        public void HigherPriceFirstWithBidComparer()
        {
            var low = new PriceTimePriority(_less, _after);
            var high = new PriceTimePriority(_more, _after);
            var comparer = new PriceTimePriorityBidComparer();

            var res = comparer.Compare(high, low);

            Assert.That(res, Is.LessThan(0));
        }

        [Test]
        public void BeforeTimeFirstWithBidComparer()
        {
            var low = new PriceTimePriority(_less, _before);
            var high = new PriceTimePriority(_less, _after);
            var comparer = new PriceTimePriorityBidComparer();

            var res = comparer.Compare(low, high);

            Assert.That(res, Is.LessThan(0));
        }
    }
}
