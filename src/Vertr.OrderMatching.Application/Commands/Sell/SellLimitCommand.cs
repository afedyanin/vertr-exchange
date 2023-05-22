using MediatR;

namespace Vertr.OrderMatching.Application.Commands.Sell
{
    public record class SellLimitCommand : IRequest
    {
        public Guid CorrelationId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public decimal Price { get; }

        public SellLimitCommand(Guid correlationId,
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
