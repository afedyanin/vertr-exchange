using Vertr.OrderMatching.Domain.Contracts;

namespace Vertr.OrderMatching.Domain.Entities
{
    public class Trade : IEntity<Guid>
    {
        public Guid Id { get; }

        public string Ticker { get; } = string.Empty;

        public DateTime TradeTime { get; }

        public decimal Price { get; }

        public decimal Qty { get; }

        private Trade()
        {
        }

        internal Trade(
            Guid id,
            string ticker,
            decimal price,
            decimal qty,
            DateTime tradeTime)
        {
            Id = id;
            Ticker = ticker;
            Price = price;
            Qty = qty;
            TradeTime = tradeTime;
        }
    }
}
