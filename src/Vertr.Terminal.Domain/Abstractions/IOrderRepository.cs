using Vertr.Terminal.Domain.OrderManagement;

namespace Vertr.Terminal.Domain.Abstractions;

public interface IOrderRepository
{
    Task<bool> AddOrder(Order order);

    Task AddEvent(OrderEvent orderEvent);

    Task<bool> Remove(long orderId);

    Task<Order[]> GetList();

    Task<Order?> GetById(long orderId);

    Task Reset();
}
