using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public class OrderBookRequest : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.ORDER_BOOK_REQUEST;

    public int Symbol { get; }

    public int Size { get; }

    public OrderBookRequest(
        long orderId,
        DateTime timestamp,
        int symbol,
        int size) : base(orderId, timestamp)
    {
        Symbol = symbol;
        Size = size;
    }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Symbol = Symbol;
        command.Size = Size;
    }
}
