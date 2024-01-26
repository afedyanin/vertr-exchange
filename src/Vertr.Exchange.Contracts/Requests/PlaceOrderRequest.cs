using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Exchange.Contracts.Requests;

public record PlaceOrderRequest
{
    public long UserId { get; set; }

    public int Symbol { get; set; }

    public decimal Price { get; set; }

    public long Size { get; set; }

    public OrderAction Action { get; set; }

    public OrderType OrderType { get; set; }
}
