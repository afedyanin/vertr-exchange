using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class Order : IOrder
{
    public OrderAction Action { get; }

    public long OrderId { get; }

    public long Price { get; }

    public long Size { get; }

    public long Filled { get; }

    public long Uid { get; }

    public long Timestamp { get; }
}
