using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Server.OrderManagement;

namespace Vertr.Terminal.Server.Repositories;

public interface IOrdersRepository
{
    Task<bool> AddOrder(Order order);

    Task AddEvent(OrderEvent orderEvent);

    Task<bool> Remove(long orderId);

    Task<Order[]> GetList();

    Task<Order?> GetById(long orderId);

    Task Reset();
}
