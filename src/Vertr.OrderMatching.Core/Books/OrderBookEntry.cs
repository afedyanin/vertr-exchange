namespace Vertr.OrderMatching.Core.Books
{
    public readonly record struct OrderBookEntry
    {
        public long OrderId { get; }

        public DateTime ArrivalTime { get; }

        public decimal Price { get; }

        public decimal RemainingQty { get; }

        public bool IsBid { get; }

        public bool IsAsk => !IsBid;

        public bool IsFilled => RemainingQty == decimal.Zero;

        public bool IsMarket => Price == decimal.Zero;

        public bool IsLimit => !IsMarket;

        public OrderBookEntry(
            long orderId,
            DateTime arrivalTime,
            decimal price,
            decimal remainingQty,
            bool isBid)
        {
            OrderId = orderId;
            ArrivalTime = arrivalTime;
            Price = price;
            RemainingQty = remainingQty;
            IsBid = isBid;
        }
    }
}
