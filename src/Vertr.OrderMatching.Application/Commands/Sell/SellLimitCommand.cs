using MediatR;
using Vertr.OrderMatching.Application.Common;

namespace Vertr.OrderMatching.Application.Commands.Sell
{
    public record class SellLimitCommand : IRequest<BuySellCommandResult>
    {
        public Guid CorrelationId { get; }

        public Guid OwnerId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public decimal Price { get; }

        public SellLimitCommand(
            Guid correlationId,
            Guid ownerId,
            string instrument,
            decimal qty,
            decimal price)
        {
            CorrelationId = correlationId;
            OwnerId = ownerId;
            Instrument = instrument;
            Qty = qty;
            Price = price;
        }
    }
}
