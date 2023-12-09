using Vertr.Exchange.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Commands;
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
