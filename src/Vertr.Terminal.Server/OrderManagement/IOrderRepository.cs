namespace Vertr.Terminal.Server.OrderManagement;

public interface IOrderRepository
{
    Task<bool> Add(Order order);

    Task<bool> Remove(long orderId);

    Task<Order[]> GetList();

    Task<Order?> GetById(long orderId);

    Task Reset();
}
