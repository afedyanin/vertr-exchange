using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.Events.OrderEvents;

public record class PlaceOrderEvent : OrderCommandEvent
{
    public long Price { get; set; }

    public long Size { get; set; }

    public long OrderId { get; set; }

    public OrderAction Action { get; set; }

    public OrderType OrderType { get; set; }

    public long Uuid { get; set; }

    public int Symbol { get; set; }

    public long ReservePrice { get; set; }

    public int UserId { get; set; }
}
