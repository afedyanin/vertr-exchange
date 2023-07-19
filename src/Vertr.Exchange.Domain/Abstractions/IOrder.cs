namespace Vertr.Exchange.Domain.Abstractions;
public interface IOrder
{
    long OrderId { get; }

    long Price { get; }

    long Size { get; }

    long Filled { get; set; }

    long Uid { get; }

    long Timestamp { get; }

    long Remaining => Size - Filled;
}
