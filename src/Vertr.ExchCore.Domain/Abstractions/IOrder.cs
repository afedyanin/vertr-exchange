using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.Abstractions;

public interface IOrder
{
    long Price { get; }

    long Size { get; } 

    long Filled { get; }

    long Uid { get; }

    OrderAction Action { get; }

    long OrderId { get; }

    long Timestamp { get; }

    long ReserveBidPrice { get; }
}
