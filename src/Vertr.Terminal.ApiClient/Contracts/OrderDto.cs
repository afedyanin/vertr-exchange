using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Contracts;

public record class OrderDto
{
    public OrderEvent[] OrderEvents { get; init; } = [];

    public long OrderId { get; init; }

    public long UserId { get; init; }

    public int Symbol { get; init; }

    public decimal Price { get; init; }

    public long Size { get; init; }

    public OrderAction Action { get; init; }

    public OrderType OrderType { get; init; }
}
