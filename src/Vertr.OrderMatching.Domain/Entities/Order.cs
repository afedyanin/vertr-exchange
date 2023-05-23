using Vertr.OrderMatching.Domain.Contracts;

namespace Vertr.OrderMatching.Domain.Entities
{
    public class Order : IEntity<Guid>
    {
        public Guid Id { get; }

        public string Instrument { get; } = string.Empty;

        public int TraderId { get; }

        // Market order if Zero
        public decimal Price { get; }

        public decimal Qty { get; }

        public decimal RemainingQty { get; private set; }

        public bool IsBuy { get; }

        public DateTime CreationTime { get; }

        public bool IsExecuted => RemainingQty <= 0;

        internal Order(Guid id,
            string instrument,
            int traderId,
            decimal price,
            decimal qty,
            bool isBuy,
            DateTime creationTime)
        {
            Id = id;
            Instrument = instrument;
            TraderId = traderId;
            IsBuy = isBuy;
            Price = price;
            Qty = qty;
            RemainingQty = qty;
            CreationTime = creationTime;
        }

        private bool IsValid()
        {
            return true;
        }
    }
}
