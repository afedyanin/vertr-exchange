using MediatR;
using Vertr.OrderMatching.Application.Common;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public record class BuyMarketCommand : IRequest<BuySellCommandResult>
    {
        public Guid CorrelationId { get; }

        public Guid OwnerId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public BuyMarketCommand(
            Guid correlationId,
            Guid ownerId,
            string instrument,
            decimal qty)
        {
            CorrelationId = correlationId;
            OwnerId = ownerId;
            Instrument = instrument;
            Qty = qty;
        }
    }
}
