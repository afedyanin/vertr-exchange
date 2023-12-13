using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.Server.Repositories;

public interface IOrdersRepository
{
    Task<bool> Add(Order order);

    Task<bool> Remove(long orderId);

    Task<Order[]> GetList();

    Task<Order?> GetById(long orderId);

    Task Reset();
}
