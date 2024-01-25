using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.MatchingEngine.Tests.Stubs;

internal static class OrderCommandStub
{
    private const int defaultSymbol = 1;

    public static OrderCommand GtcOrder(
        OrderAction action,
        long orderId,
        long uid,
        decimal price,
        long size,
        int symbol = defaultSymbol)
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
            Size = size,
            Symbol = symbol,
        };
    }

    public static OrderCommand IocOrder(
        OrderAction action,
        long orderId,
        long uid,
        decimal price,
        long size,
        int symbol = defaultSymbol)
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
            Size = size,
            Symbol = symbol,
        };
    }

    public static OrderCommand MoveOrder(
        long orderId,
        long uid,
        decimal price,
        int symbol = defaultSymbol)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.MOVE_ORDER,
            OrderId = orderId,
            Uid = uid,
            Price = price,
            Size = 0L,
            Symbol = symbol,
        };
    }

    public static OrderCommand Reduce(
        long orderId,
        long uid,
        long size,
        int symbol = defaultSymbol)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.REDUCE_ORDER,
            OrderId = orderId,
            Uid = uid,
            Size = size,
            Symbol = symbol,
        };
    }

    public static OrderCommand Reject(
        long orderId,
        long uid,
        decimal price,
        long size,
        int symbol = defaultSymbol)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.PLACE_ORDER,
            ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE,
            OrderType = OrderType.FOK_BUDGET, // unsupported order type
            OrderId = orderId,
            Uid = uid,
            Price = price,
            Size = size,
            Symbol = symbol,
        };
    }

    public static OrderCommand Cancel(
        long orderId,
        long uid,
        int symbol = defaultSymbol)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.CANCEL_ORDER,
            OrderId = orderId,
            Uid = uid,
            Symbol = symbol,
        };
    }

    public static OrderCommand OrderBookRequest(int size, int symbol = defaultSymbol)
    {
        return new OrderCommand
        {
            Command = OrderCommandType.ORDER_BOOK_REQUEST,
            Size = size,
            Symbol = symbol,
        };
    }
}
