namespace Vertr.OrderMatching.Application.Notifications.NewTrade
{
    public record class NewTradeNotification
    {
        public Guid Id { get; }

        public long TradeSequentialNumber { get; }

        public Guid SellOrderId { get; }

        public Guid BuyOrderId { get; }

        public DateTime TradeTime { get; }

        public decimal Price { get; }

        public decimal Qty { get; }

        public NewTradeNotification(
            Guid id,
            long tradeSequentialNumber,
            Guid sellOrderId,
            Guid buyOrderId,
            DateTime tradeTime,
            decimal price,
            decimal qty)
        {
            Id = id;
            TradeSequentialNumber = tradeSequentialNumber;
            SellOrderId = sellOrderId;
            BuyOrderId = buyOrderId;
            TradeTime = tradeTime;
            Price = price;
            Qty = qty;
        }
    }
}
