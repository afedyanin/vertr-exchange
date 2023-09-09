using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public record class PlaceOrderCommand : ApiCommand
{
    public decimal Price { get; set; }

    public long Size { get; set; }

    public long OrderId { get; set; }

    public OrderAction Action { get; set; }

    public OrderType OrderType { get; set; }

    public long Uid { get; set; }

    public int Symbol { get; set; }
}
