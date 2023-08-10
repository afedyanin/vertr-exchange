using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;

internal static class OrderCommandStub
{
    public static OrderCommand MoveOrder(
        long orderId,
        long uid,
        decimal price,
        long size)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.MOVE_ORDER,
            OrderId = orderId,
            Uid = uid,
            Price = price,
            Size = size
        };
    }
}
