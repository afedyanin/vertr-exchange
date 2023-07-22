using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain.MatchingEngine;

internal sealed class Order : IOrder
{
    public OrderAction Action { get; set; }

    public long OrderId { get; set; }

    public long Price { get; set; }

    public long Size { get; set; }

    public long Filled { get; set; }

    public long Uid { get; set; }

    public long Timestamp { get; set; }
}
