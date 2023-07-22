namespace Vertr.OrderMatching.Core.Books
{
    public class OrderBook
    {
        private readonly MarketDepth _bids = new(isBid: true);
        private readonly MarketDepth _asks = new(isBid: false);

        public MarketDepth Bids => _bids;
        public MarketDepth Asks => _asks;
    }
}
