using MediatR;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public record class BuyMarketCommand : IRequest
    {
        public Guid CorrelationId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public BuyMarketCommand(
            Guid correlationId,
            string instrument,
            decimal qty)
        {
            CorrelationId = correlationId;
            Instrument = instrument;
            Qty = qty;
        }
    }
}
