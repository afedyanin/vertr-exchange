using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public interface IOrderRepository
    {
        Order[] GetAll();

        Order? GetById(Guid orderId);

        bool Insert(Order order);

        bool Delete(Guid orderId);
    }
}
