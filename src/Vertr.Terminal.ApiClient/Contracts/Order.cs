using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Contracts;

public class Order(
    long orderId,
    long userId,
    int symbol,
    decimal price,
    long size,
    OrderAction action,
    OrderType orderType
        )
{
    private readonly List<OrderEvent> _events = [];

    public long OrderId { get; } = orderId;

    public long UserId { get; } = userId;

    public int Symbol { get; } = symbol;

    public decimal Price { get; } = price;

    public long Size { get; } = size;

    public OrderAction Action { get; } = action;

    public OrderType OrderType { get; } = orderType;

    public void AddEvent(OrderEvent orderEvent)
    {
        _events.Add(orderEvent);
    }

    public OrderEvent[] GetEvents() => _events.ToArray();
}
