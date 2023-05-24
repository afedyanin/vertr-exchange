namespace Vertr.OrderMatching.Core.Trades
{
    public readonly record struct OrderFullfilment
    {
        public Guid BuyOrderId { get; }

        public Guid SellOrderId { get; }

        public decimal Price { get; }

        public decimal FilledQty { get; }

        public OrderFullfilment(
            Guid buyOrderId,
            Guid sellOrderId,
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
