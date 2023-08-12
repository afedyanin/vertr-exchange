using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;

internal static class OrderCommandStub
{
    public static OrderCommand GtcOrder(
        OrderAction action,
        long orderId,
        long uid,
        decimal price,
        long size)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.PLACE_ORDER,
            ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE,
            OrderType = OrderType.GTC,
            Action = action,
            OrderId = orderId,
            Uid = uid,
            Price = price,
            Size = size
        };
    }

    public static OrderCommand IocOrder(
        OrderAction action,
        long orderId,
        long uid,
        decimal price,
        long size)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.PLACE_ORDER,
            ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE,
            OrderType = OrderType.IOC,
            Action = action,
            OrderId = orderId,
            Uid = uid,
            Price = price,
            Size = size
        };
    }

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

    public static OrderCommand Reduce(
        long orderId,
        long uid,
        long size)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.REDUCE_ORDER,
            OrderId = orderId,
            Uid = uid,
            Size = size
        };
    }

    public static OrderCommand Reject(
        long orderId,
        long uid,
        decimal price,
        long size)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.PLACE_ORDER,
            ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE,
            OrderType = OrderType.FOK_BUDGET, // unsupported order type
            OrderId = orderId,
            Uid = uid,
            Price = price,
            Size = size
        };
    }

    public static OrderCommand Cancel(
        long orderId,
        long uid)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.CANCEL_ORDER,
            OrderId = orderId,
            Uid = uid,
        };
    }

    public static OrderCommand OrderBookRequest(int size)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.ORDER_BOOK_REQUEST,
            Size = size,
        };
    }
}
