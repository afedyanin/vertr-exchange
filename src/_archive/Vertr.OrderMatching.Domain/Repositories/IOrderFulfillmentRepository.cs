using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public interface IOrderFulfillmentRepository
    {
        OrderFulfillment[] GetAll();

        OrderFulfillment? GetById(Guid orderId);

        bool Insert(OrderFulfillment order);

        bool Delete(Guid orderId);
    }
}
