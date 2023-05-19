namespace Vertr.OrderMatching.Core.Trades
{
    public readonly record struct OrderFullfilment
    {
        public long BuyOrderId { get; }

        public long SellOrderId { get; }

        public decimal Price { get; }

        public decimal FilledQty { get; }

        public OrderFullfilment(
            long buyOrderId,
            long sellOrderId,
            decimal price,
            decimal filledQty)
        {
            BuyOrderId = buyOrderId;
            SellOrderId = sellOrderId;
            Price = price;
            FilledQty = filledQty;
        }
    }
}
