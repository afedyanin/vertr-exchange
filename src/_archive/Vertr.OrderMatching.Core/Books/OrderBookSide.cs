using System.Diagnostics;
using Vertr.OrderMatching.Core.Priority;

namespace Vertr.OrderMatching.Core.Books
{
    public class OrderBookSide
    {
        private readonly PriorityQueue<OrderBookEntry, PriceTimePriority> _entries;
        private readonly IComparer<PriceTimePriority> _comparer;

        public int Count => _entries.Count;

        public bool IsEmpty => _entries.Count <= 0;

        public bool IsMarketBook { get; }

        public bool IsBid { get; }

        internal OrderBookSide(bool isBid, bool isMarketBook)
        {
            _comparer = isMarketBook ?
                PriceTimeComparers.MarketComparer :
                isBid ?
                    PriceTimeComparers.BidComparer :
                    PriceTimeComparers.AskComparer;

            _entries = new(_comparer);
            IsMarketBook = isMarketBook;
            IsBid = isBid;
        }

        public bool Peek(out OrderBookEntry entry) => _entries.TryPeek(out entry, out var _);

        public OrderBookEntry Take() => _entries.Dequeue();

        public void Place(OrderBookEntry entry)
        {
            Debug.Assert(entry.IsBid == IsBid);
            Debug.Assert(entry.IsMarket == IsMarketBook);

            if (entry.IsFilled)
            {
                return;
            }

            _entries.Enqueue(entry, entry.GetPriority());
        }

        public OrderBookEntry[] GetSnapshot()
        {
            return _entries.UnorderedItems
                .OrderBy(e => e.Priority, _comparer)
                .Select(e => e.Element)
                .ToArray();
        }
    }
}
