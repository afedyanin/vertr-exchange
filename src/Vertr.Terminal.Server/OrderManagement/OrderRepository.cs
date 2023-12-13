
namespace Vertr.Terminal.Server.OrderManagement;

public class OrderRepository : IOrderRepository
{
    private readonly Dictionary<long, Order> _orders = [];

    public Task<Order?> GetById(long orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }

    public Task<Order[]> GetList()
    {
        var res = _orders.Values.OrderBy(ord => ord.OrderId).ToArray();
        return Task.FromResult(res);
    }

    public Task Reset()
    {
        _orders.Clear();
        return Task.CompletedTask;
    }

    public Task<bool> Add(Order order)
    {
        var res = _orders.TryAdd(order.OrderId, order);
        return Task.FromResult(res);
    }

    public Task<bool> Remove(long orderId)
    {
        var res = _orders.Remove(orderId);
        return Task.FromResult(res);
    }
}
