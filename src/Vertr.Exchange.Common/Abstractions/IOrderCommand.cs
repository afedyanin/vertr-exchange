using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;
public interface IOrderCommand : IOrder
{
    OrderCommandType Command { get; }

    CommandResultCode ResultCode { get; }

    OrderType OrderType { get; }
}
