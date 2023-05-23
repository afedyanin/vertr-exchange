using MediatR;
using Vertr.OrderMatching.Application.Common;

namespace Vertr.OrderMatching.Application.Commands.Buy
{
    public record class BuyLimitCommand : IRequest<BuySellCommandResult>
    {
        public Guid CorrelationId { get; }

        public Guid OwnerId { get; }

        public string Instrument { get; }

        public decimal Qty { get; }

        public decimal Price { get; }

        public BuyLimitCommand(Guid correlationId,
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
