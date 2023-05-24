using Vertr.OrderMatching.Core.Books;

namespace Vertr.OrderMatching.Core.Tests
{
    public class OrderMatcherTests
    {
        private readonly DateTime _time = new(2023, 05, 17, 20, 00, 00, DateTimeKind.Utc);

        private readonly DateTime _before = new(2023, 05, 17, 20, 00, 00, DateTimeKind.Utc);
        private readonly DateTime _after = new(2023, 05, 17, 20, 10, 00, DateTimeKind.Utc);

        private readonly decimal _more = 15.56m;
        private readonly decimal _less = 12.48m;

        [Test]
        public void CanFillBidAndAskWithTheSameQty()
        {
            var bid = new OrderBookEntry(10L, _time, 12.0m, 50, true);
            var ask = new OrderBookEntry(12L, _time, 12.0m, 50, false);

            var trade = OrderMatcher.FillSingle(ref bid, ref ask);

            Assert.Multiple(() =>
            {
                Assert.That(trade.FilledQty, Is.EqualTo(50));
                Assert.That(ask.IsFilled, Is.EqualTo(true));
                Assert.That(bid.IsFilled, Is.EqualTo(true));
            });
        }

        [TestCase(20, 20, 20, 0, 0)]
        [TestCase(30, 20, 20, 10, 0)]
        [TestCase(20, 30, 20, 0, 10)]
        [TestCase(30, 0, 0, 30, 0)]
        [TestCase(0, 20, 0, 0, 20)]
        [TestCase(0, 0, 0, 0, 0)]
        public void CanFillBidAndAskWithDiffQty(
            decimal bidQty,
            decimal askQty,
            decimal tradeQty,
            decimal bidRestQty,
            decimal askRestQty
            )
        {
            var bid = new OrderBookEntry(10L, _time, 12.0m, bidQty, true);
            var ask = new OrderBookEntry(12L, _time, 12.0m, askQty, false);

            var trade = OrderMatcher.FillSingle(ref bid, ref ask);

            Assert.Multiple(() =>
            {
                Assert.That(trade.FilledQty, Is.EqualTo(tradeQty));
                Assert.That(bid.RemainingQty, Is.EqualTo(bidRestQty));
                Assert.That(ask.RemainingQty, Is.EqualTo(askRestQty));
            });
        }

        [TestCase(0, 0, 12, 13, false)]
        [TestCase(0, 10, 12, 13, false)]
        [TestCase(10, 0, 12, 13, false)]
        [TestCase(10, 10, 12, 13, false)]
        [TestCase(10, 10, 12, 11, true)]
        [TestCase(10, 10, 0, 13, true)]
        [TestCase(10, 10, 12, 0, true)]
        [TestCase(10, 10, 12, 12, true)]
        public void CanCheckFill(
            decimal bidQty,
            decimal askQty,
            decimal bidPrice,
            decimal askPrice,
            bool canFill
            )
        {
            var bid = new OrderBookEntry(10L, _time, bidPrice, bidQty, true);
            var ask = new OrderBookEntry(12L, _time, askPrice, askQty, false);

            var res = OrderMatcher.CanFill(ref bid, ref ask);

            Assert.That(res, Is.EqualTo(canFill));
        }

        [Test]
        public void CanMatchMarketBid()
        {
            var book = new OrderBook();

            // use Id as order to dequeue
            var ask1 = new OrderBookEntry(2L, _after, _less, 10, false);
            var ask2 = new OrderBookEntry(4L, _after, _more, 20, false);
            var ask3 = new OrderBookEntry(3L, _before, _more, 30, false);
            var ask4 = new OrderBookEntry(1L, _before, _less, 40, false);

            book.Asks.Place(ask1);
            book.Asks.Place(ask2);
            book.Asks.Place(ask3);
            book.Asks.Place(ask4);

            var bid1 = new OrderBookEntry(40L, _after, decimal.Zero, 43, true);

            var trades = OrderMatcher.MatchBid(ref bid1, book);

            Assert.Multiple(() =>
            {
                Assert.That(trades, Has.Length.EqualTo(2));
                Assert.That(bid1.IsFilled, Is.EqualTo(true));
            });

            var sn = book.Asks.Limit.GetSnapshot();

            foreach (var item in sn)
            {
                Console.WriteLine(item);
            }

            Assert.Pass();
        }

        [Test]
        public void CanMatchLimitBid()
        {
            var book = new OrderBook();

            // use Id as order to dequeue
            var ask1 = new OrderBookEntry(2L, _after, _less, 10, false);
            var ask2 = new OrderBookEntry(4L, _after, _more, 20, false);
            var ask3 = new OrderBookEntry(3L, _before, _more, 30, false);
            var ask4 = new OrderBookEntry(1L, _before, _less, 40, false);

            book.Asks.Place(ask1);
            book.Asks.Place(ask2);
            book.Asks.Place(ask3);
            book.Asks.Place(ask4);

            var bid1 = new OrderBookEntry(40L, _after, _more + 0.1m, 63, true);

            var trades = OrderMatcher.MatchBid(ref bid1, book);
            Assert.Multiple(() =>
            {
                Assert.That(trades, Has.Length.EqualTo(3));
                Assert.That(bid1.IsFilled, Is.EqualTo(true));
            });

            var sn = book.Asks.Limit.GetSnapshot();

            foreach (var item in sn)
            {
                Console.WriteLine(item);
            }

            Assert.Pass();
        }
    }
}
