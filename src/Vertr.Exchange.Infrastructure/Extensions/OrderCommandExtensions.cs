using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Infrastructure.Extensions;
internal static class OrderCommandExtensions
{
    private static OrderCommand _emptyCommand { get; }
        = new OrderCommand()
        {
            Action = OrderAction.ASK,
            BinaryCommandType = BinaryDataType.NONE,
            BinaryData = Array.Empty<byte>(),
            Command = OrderCommandType.NOP,
            EngineEvent = null,
            Filled = long.MinValue,
            MarketData = null,
            OrderId = long.MinValue,
            OrderType = OrderType.GTC,
            Price = decimal.MinValue,
            ResultCode = CommandResultCode.NEW,
            Size = long.MinValue,
            Symbol = int.MinValue,
            Timestamp = DateTime.MinValue,
            Uid = long.MinValue,
        };

    public static void Clean(this OrderCommand cmd)
    {
        cmd.Fill(_emptyCommand);
    }

    public static void Fill(this OrderCommand cmd, OrderCommand src)
    {
        cmd.Action = src.Action;
        cmd.BinaryCommandType = src.BinaryCommandType;
        cmd.BinaryData = src.BinaryData;
        cmd.Command = src.Command;
        cmd.EngineEvent = src.EngineEvent;
        cmd.Filled = src.Filled;
        cmd.MarketData = src.MarketData;
        cmd.OrderId = src.OrderId;
        cmd.OrderType = src.OrderType;
        cmd.Price = src.Price;
        cmd.ResultCode = src.ResultCode;
        cmd.Size = src.Size;
        cmd.Symbol = src.Symbol;
        cmd.Timestamp = src.Timestamp;
        cmd.Uid = src.Uid;
    }
}
