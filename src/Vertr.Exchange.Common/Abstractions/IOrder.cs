using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;
public interface IOrder
{
    OrderAction Action { get; }

    long OrderId { get; }

    decimal Price { get; }

    long Size { get; }

    long Filled { get; }

    long Uid { get; }

    DateTime Timestamp { get; }

    long Remaining { get; }

    bool Completed { get; }

    void ReduceSize(long reduceBy);

    void Fill(long increment);

    void SetPrice(decimal price);
}
