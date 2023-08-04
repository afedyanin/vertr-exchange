using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;
public interface IOrder
{
    OrderAction Action { get; }

    long OrderId { get; }

    long Price { get; }

    long Size { get; }

    long Filled { get; }

    long Uid { get; }

    long Timestamp { get; }

    long Remaining { get; }

    bool Completed { get; }

    void ReduceSize(long reduceBy);

    void Fill(long increment);

    void SetPrice(long price);
}
