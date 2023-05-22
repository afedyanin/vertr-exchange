using MediatR;

namespace Vertr.OrderMatching.Application.Commands.Sell
{
    public record class SellMarketCommand : IRequest
    {
        public Guid CorrelationId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public SellMarketCommand(
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
