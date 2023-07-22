using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public class OrderFulfillmentRepository : EntityInMemoryRepository<OrderFulfillment>, IOrderFulfillmentRepository
    {
    }
}
