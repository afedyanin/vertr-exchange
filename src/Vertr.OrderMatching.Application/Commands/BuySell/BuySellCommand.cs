using MediatR;

namespace Vertr.OrderMatching.Application.Commands.BuySell
{
    public record class BuySellCommand : IRequest<BuySellCommandResult>
    {
        public Guid CorrelationId { get; }

        public Guid OwnerId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public decimal Price { get; }

        public bool IsBuy { get; }

        public BuySellCommand(Guid correlationId,
            Guid ownerId,
            string instrument,
            decimal qty,
            decimal price,
            bool isBuy)
        {
            CorrelationId = correlationId;
            OwnerId = ownerId;
            Instrument = instrument;
            Qty = qty;
            Price = price;
            IsBuy = isBuy;  
        }
    }
}
