using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Application.Commands;
public class OrderBookRequest(
    long orderId,
    DateTime timestamp,
    int symbol,
    int size)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.ORDER_BOOK_REQUEST;

    public int Symbol { get; } = symbol;

    public int Size { get; } = size;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Symbol = Symbol;
        command.Size = Size;
        command.Uid = 0L;
    }
}
