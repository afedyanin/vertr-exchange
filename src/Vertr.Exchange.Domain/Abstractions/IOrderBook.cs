using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain.Abstractions;

public interface IOrderBook
{
    CommandResultCode ProcessCommand(OrderCommand cmd);
}
