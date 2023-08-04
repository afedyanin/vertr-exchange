using System.Diagnostics;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class Order : IOrder
{
    public OrderAction Action { get; }

    public long OrderId { get; }

    public long Price { get; private set; }

    public long Size { get; private set; }

    public long Filled { get; private set; }

    public long Uid { get; }

    public long Timestamp { get; }

    public long Remaining => Size - Filled;

    public bool Completed => Remaining <= 0L;

    public Order(
        OrderAction action,
        long orderId,
        long price,
        long size,
        long filled,
        long uid,
        long timestamp
        )
    {
        Action = action;
        OrderId = orderId;
        Price = price;
        Size = size;
        Filled = filled;
        Uid = uid;
        Timestamp = timestamp;
    }

    public void ReduceSize(long reduceBy)
    {
        Debug.Assert(reduceBy > 0L);
        Debug.Assert(reduceBy <= Remaining);
        Size -= reduceBy;
    }

    public void Fill(long increment)
    {
        Debug.Assert(increment > 0L);
        Debug.Assert(increment <= Remaining);
        Filled += increment;
    }

    public void SetPrice(long price)
    {
        Price = price;
    }
}
