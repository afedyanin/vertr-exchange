using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Contracts
{
    public interface IOrderFactory
    {
        Order CreateOrder(
            Guid correlationId,
            Guid ownerId,
            string instrument,
            decimal qty,
            decimal price,
            bool isBuy);
    }
}
