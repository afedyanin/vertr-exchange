namespace Vertr.OrderMatching.Core.Priority
{
    internal readonly struct PriceTimePriority
    {
        public decimal Price { get; init; }

        public DateTime Time { get; init; }

        public PriceTimePriority(decimal price, DateTime time)
        {
            Price = price;
            Time = time;
        }
    }
}
