using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine;

internal sealed class Order : IOrder
{
    public OrderAction Action { get; }

    public long OrderId { get; }

    public decimal Price { get; private set; }

    public decimal ReserveBidPrice { get; set; }

    public long Size { get; private set; }

    public long Filled { get; private set; }

    public long Uid { get; }

    public DateTime Timestamp { get; }

    public long Remaining => Size - Filled;

    public bool Completed => Remaining <= 0L;

    public Order(
        OrderAction action,
        long orderId,
        decimal price,
        decimal reserveBidPrice,
        long size,
        long filled,
        long uid,
        DateTime timestamp
        )
    {
        Validate(price, size, filled);

        Action = action;
        OrderId = orderId;
        Price = price;
        ReserveBidPrice = reserveBidPrice; // TODO: Add validation
        Size = size;
        Filled = filled;
        Uid = uid;
        Timestamp = timestamp;
    }

    public void ReduceSize(long reduceBy)
    {
        if (reduceBy < 0L)
        {
            throw new ArgumentOutOfRangeException(nameof(reduceBy), "Reduce value cannot be < 0.");
        }

        if (reduceBy > Remaining)
        {
            throw new InvalidOperationException($"Reduce={reduceBy} cannot exceed order Remaining={Remaining}.");
        }

        Size -= reduceBy;
    }

    public void Fill(long increment)
    {
        if (increment < 0L)
        {
            throw new ArgumentOutOfRangeException(nameof(increment), "Increment value cannot be < 0.");
        }

        if (increment > Remaining)
        {
            throw new InvalidOperationException($"Increment={increment} cannot exceed order Remaining={Remaining}.");
        }

        Filled += increment;
    }

    public void SetPrice(decimal price)
    {
        if (price < decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price value cannot be < 0.");
        }

        Price = price;
    }

    public void Update(decimal price, long size, long filled = 0L)
    {
        Validate(price, size, filled);

        Price = price;
        Size = size;
        Filled = filled;
    }

    private void Validate(decimal price, long size, long filled = 0L)
    {
        if (price < decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price value cannot be < 0.");
        }

        if (size <= 0L)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size value cannot be < 0.");
        }

        if (filled < 0L)
        {
            throw new ArgumentOutOfRangeException(nameof(filled), "Filled value cannot be < 0.");
        }

        if (size < filled)
        {
            throw new InvalidOperationException($"Filled={filled} cannot exceed order Size={size}.");
        }
    }
}
