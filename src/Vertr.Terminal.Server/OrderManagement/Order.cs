using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.Server.OrderManagement;

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
    public OrderEvent[] OrderEvents { get; private set; } = [];

    public long OrderId { get; } = orderId;

    public long UserId { get; } = userId;

    public int Symbol { get; } = symbol;

    public decimal Price { get; } = price;

    public long Size { get; } = size;

    public OrderAction Action { get; } = action;

    public OrderType OrderType { get; } = orderType;

    public void SetEvents(OrderEvent[] orderEvents)
    {
        OrderEvents = orderEvents ?? [];
    }
}
