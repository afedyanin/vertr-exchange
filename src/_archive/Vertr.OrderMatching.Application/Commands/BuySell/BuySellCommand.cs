using MediatR;

namespace Vertr.OrderMatching.Application.Commands.BuySell
{
    public record class BuySellCommand : IRequest<BuySellCommandResult>
    {
        public Guid CorrelationId { get; }

        public Guid OwnerId { get; }

        public string Ticker { get; }

        public decimal Qty { get; }

        public decimal Price { get; }

        public bool IsBuy { get; }

        public BuySellCommand(Guid correlationId,
            Guid ownerId,
            string ticker,
            decimal qty,
            decimal price,
            bool isBuy)
        {
            CorrelationId = correlationId;
            OwnerId = ownerId;
            Ticker = ticker;
            Qty = qty;
            Price = price;
            IsBuy = isBuy;
        }
    }
}
