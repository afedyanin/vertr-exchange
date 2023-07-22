using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Contracts
{
    public interface IOrderFulfillmentFactory
    {
        OrderFulfillment CreateOrderFulfillment(
            Guid orderId,
            Guid tradeId,
            decimal price,
            decimal filledQty,
            decimal remainigQty);
    }
}
