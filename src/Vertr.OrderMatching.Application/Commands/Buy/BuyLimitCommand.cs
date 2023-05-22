using MediatR;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public record class BuyLimitCommand : IRequest
    {
        public Guid CorrelationId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public decimal Price { get; }

        public BuyLimitCommand(Guid correlationId,
            string instrument,
            decimal qty,
            decimal price)
        {
            CorrelationId = correlationId;
            Instrument = instrument;
            Qty = qty;
            Price = price;
        }
    }
}
