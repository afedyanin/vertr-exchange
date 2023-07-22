using Vertr.OrderMatching.Core.Books;

namespace Vertr.OrderMatching.Core.Priority
{
    internal static class PriceTimeComparers
    {
        private static readonly IComparer<PriceTimePriority> _bidComparer = new PriceTimePriorityBidComparer();

        private static readonly IComparer<PriceTimePriority> _askComparer = new PriceTimePriorityAskComparer();

        private static readonly IComparer<PriceTimePriority> _marketComparer = new PriceTimePriorityMarketComparer();

        public static IComparer<PriceTimePriority> BidComparer => _bidComparer;

        public static IComparer<PriceTimePriority> AskComparer => _askComparer;

        public static IComparer<PriceTimePriority> MarketComparer => _marketComparer;

        public static PriceTimePriority GetPriority(this OrderBookEntry entry)
        {
            return new PriceTimePriority(entry.Price, entry.ArrivalTime);
        }
    }
}
