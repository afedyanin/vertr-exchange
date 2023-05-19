using System.Security.Cryptography;
using Vertr.OrderMatching.Core.Books;

namespace Vertr.OrderMatching.Core.Tests
{
    public class OrderBookTests
    {
        private readonly DateTime _before = new(2023, 05, 17, 20, 00, 00, DateTimeKind.Utc);
        private readonly DateTime _after = new(2023, 05, 17, 20, 10, 00, DateTimeKind.Utc);

        private readonly decimal _more = 15.56m;
        private readonly decimal _less = 12.48m;


        [Test]
        public void CanGetBidsCount()
        {
            var book = new OrderBook();

            // use Id as order to dequeue
            var bid1 = new OrderBookEntry(4L, _after, _less, 10, true);
            var bid2 = new OrderBookEntry(2L, _after, _more, 20, true);
            var bid3 = new OrderBookEntry(1L, _before, _more, 30, true);
            var bid4 = new OrderBookEntry(3L, _before, _less, 40, true);

            book.Bids.Place(bid1);
            book.Bids.Place(bid2);
            book.Bids.Place(bid3);
            book.Bids.Place(bid4);

            Assert.That(book.Bids.Limit.Count, Is.EqualTo(4));
        }

        [Test]
        public void CanGetBidsSnapShot()
        {
            var book = new OrderBook();

            // use Id as order to dequeue
            var bid1 = new OrderBookEntry(4L, _after, _less, 10, true);
            var bid2 = new OrderBookEntry(2L, _after, _more, 20, true);
            var bid3 = new OrderBookEntry(1L, _before, _more, 30, true);
            var bid4 = new OrderBookEntry(3L, _before, _less, 40, true);

            book.Bids.Place(bid1);
            book.Bids.Place(bid2);
            book.Bids.Place(bid3);
            book.Bids.Place(bid4);

            var sn = book.Bids.Limit.GetSnapshot();

            var id = 1L;
            foreach (var item in sn)
            {
                Assert.That(item.OrderId, Is.EqualTo(id++));
                Console.WriteLine(item);
            }

            Assert.Pass();
        }

        [Test]
        public void CanGetAsksSnapShot()
        {
            var book = new OrderBook();

            // use Id as order to dequeue
            var ask1 = new OrderBookEntry(2L, _after, _less, 10, false);
            var ask2 = new OrderBookEntry(4L, _after, _more, 20, false);
            var ask3 = new OrderBookEntry(3L, _before, _more, 30, false);
            var ask4 = new OrderBookEntry(1L, _before, _less, 40, false);

            book.Asks.Limit.Place(ask1);
            book.Asks.Limit.Place(ask2);
            book.Asks.Limit.Place(ask3);
            book.Asks.Limit.Place(ask4);

            var sn = book.Asks.Limit.GetSnapshot();

            var id = 1L;
            foreach (var item in sn)
            {
                Assert.That(item.OrderId, Is.EqualTo(id++));
                Console.WriteLine(item);
            }

            Assert.Pass();
        }
    }
}
