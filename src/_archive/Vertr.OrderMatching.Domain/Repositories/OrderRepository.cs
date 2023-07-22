using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public class OrderRepository : EntityInMemoryRepository<Order>, IOrderRepository
    {
    }
}
