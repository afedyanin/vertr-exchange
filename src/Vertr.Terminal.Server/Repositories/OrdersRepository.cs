using System.Collections.Concurrent;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Server.OrderManagement;

namespace Vertr.Terminal.Server.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly ConcurrentDictionary<long, Order> _orders = [];
    private readonly ConcurrentBag<OrderEvent> _orderEvents = [];

    public Task<Order?> GetById(long orderId)
    {
        _orders.TryGetValue(orderId, out var order);
        SetEvents(order);

        return Task.FromResult(order);
    }

    public Task<Order[]> GetList()
    {
        var res = _orders.Values.OrderBy(ord => ord.OrderId).ToArray();
        res = SetEvents(res);

        return Task.FromResult(res);
    }

    public Task Reset()
    {
        _orders.Clear();
        _orderEvents.Clear();
        return Task.CompletedTask;
    }

    public Task<bool> Remove(long orderId)
    {
        var res = _orders.TryRemove(orderId, out var _);
        return Task.FromResult(res);
    }

    public Task<bool> AddOrder(Order order)
    {
        var res = _orders.TryAdd(order.OrderId, order);
        return Task.FromResult(res);
    }

    public Task AddEvent(OrderEvent orderEvent)
    {
        _orderEvents.Add(orderEvent);
        return Task.CompletedTask;
    }

    private void SetEvents(Order? order)
    {
        if (order != null)
        {
            var events = _orderEvents.Where(oe => oe.OrderId == order.OrderId).OrderBy(ev => ev.Seq).ToArray();
            order.SetEvents(events);
        }
    }

    private Order[] SetEvents(Order[] orders)
    {
        if (orders == null || orders.Length <= 0)
        {
            return orders ?? [];
        }

        foreach (var order in orders)
        {
            SetEvents(order);
        }

        return orders;
    }
}
