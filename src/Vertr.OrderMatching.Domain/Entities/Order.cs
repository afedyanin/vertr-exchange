using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.ValueObjects;

namespace Vertr.OrderMatching.Domain.Entities
{
    public class Order : IEntity<Guid>
    {
        public Guid Id { get; }

        public Guid CorrelationId { get; }

        public Guid OwnerId { get; }

        public string Instrument { get; } 

        // Market order if Zero
        public decimal Price { get; }

        public decimal Qty { get; }

        public bool IsBuy { get; }

        public DateTime CreationTime { get; }

        internal Order(Guid id,
            Guid correlationId,
            Guid ownerId,
            string instrument,
            decimal qty,
            decimal price,
            bool isBuy,
            DateTime creationTime)
        {
            Id = id;
            CorrelationId = correlationId;
            OwnerId = ownerId;
            Instrument = instrument;
            Qty = qty;
            Price = price;
            IsBuy = isBuy;
            CreationTime = creationTime;
        }

        public ValidationResult Validate()
        {
            var errors = new List<string>();

            if (Id == Guid.Empty)
            {
                errors.Add("Invalid OrderId.");
            }

            if (CorrelationId == Guid.Empty)
            {
                errors.Add("Invalid CorrelationId.");
            }

            if (OwnerId == Guid.Empty)
            {
                errors.Add("Invalid OwnerId.");
            }

            if (string.IsNullOrEmpty(Instrument))
            {
                errors.Add("Instrument cannot be empty.");
            }

            if (Price < decimal.Zero)
            {
                errors.Add("Price must be greater than or equal to zero.");
            }

            if (Qty <= decimal.Zero)
            {
                errors.Add("Quantity must be greater than zero.");
            }

            return errors.Count == 0 ?
                new ValidationResult(true):
                new ValidationResult(false, errors.ToArray());
        }
    }
}
