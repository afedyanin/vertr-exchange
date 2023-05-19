using System.Diagnostics;

namespace Vertr.OrderMatching.Core.Books
{
    public class MarketDepth
    {
        public OrderBookSide Market { get; }
        public OrderBookSide Limit { get; }

        public bool IsBid { get; }

        internal MarketDepth(bool isBid)
        {
            IsBid = isBid;
            Market = new OrderBookSide(isBid, true);
            Limit = new OrderBookSide(isBid, false);
        }

        public void Place(OrderBookEntry entry)
        {
            Debug.Assert(IsBid == entry.IsBid);

            if (entry.IsMarket)
            {
                Market.Place(entry);
            }
            else
            {
                Limit.Place(entry);
            }
        }
    }
}
