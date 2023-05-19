using Vertr.OrderMatching.Core.Books;
using Vertr.OrderMatching.Core.Priority;

namespace Vertr.OrderMatching.Core.Tests
{
    public class PriorityQueueTests
    {
        private readonly DateTime _before = new(2023, 05, 17, 20, 00, 00, DateTimeKind.Utc);
        private readonly DateTime _after = new(2023, 05, 17, 20, 10, 00, DateTimeKind.Utc);

        private readonly decimal _more = 15.56m;
        private readonly decimal _less = 12.48m;

        [Test]
        public void BidQueueSortedByHigherPrice()
        {
            var bids = new PriorityQueue<OrderBookEntry, PriceTimePriority>(PriceTimeComparers.BidComparer);

            // use Id as order to dequeue
            var bid1 = new OrderBookEntry(4L, _after, _less, 0, true);
            var bid2 = new OrderBookEntry(2L, _after, _more, 0, true);
            var bid3 = new OrderBookEntry(1L, _before, _more, 0, true);
            var bid4 = new OrderBookEntry(3L, _before, _less, 0, true);

            bids.Enqueue(bid1, bid1.GetPriority());
            bids.Enqueue(bid2, bid2.GetPriority());
            bids.Enqueue(bid3,  bid3.GetPriority());
            bids.Enqueue(bid4, bid4.GetPriority());

            var id = 1L;

            while (bids.TryDequeue(out var bid, out var _))
            {
                Assert.That(bid.OrderId, Is.EqualTo(id++));
            }
        }
        [Test]

        public void AskQueueSortedByHigherPrice()
        {
            var asks = new PriorityQueue<OrderBookEntry, PriceTimePriority>(PriceTimeComparers.AskComparer);

            // use Id as order to dequeue
            var ask1 = new OrderBookEntry(2L, _after, _less, 0, false);
            var ask2 = new OrderBookEntry(4L, _after, _more, 0, false);
            var ask3 = new OrderBookEntry(3L, _before, _more, 0, false);
            var ask4 = new OrderBookEntry(1L, _before, _less, 0, false);

            asks.Enqueue(ask1, ask1.GetPriority());
            asks.Enqueue(ask2, ask2.GetPriority());
            asks.Enqueue(ask3, ask3.GetPriority());
            asks.Enqueue(ask4, ask4.GetPriority());

            var id = 1L;

            while (asks.TryDequeue(out var ask, out var _))
            {
                Assert.That(ask.OrderId, Is.EqualTo(id++));
            }
        }
    }
}
