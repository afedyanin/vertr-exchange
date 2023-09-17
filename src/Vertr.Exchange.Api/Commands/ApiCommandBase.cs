using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;

public abstract class ApiCommandBase : IApiCommand
{
    public long OrderId { get; }

    public DateTime Timestamp { get; }

    public ApiCommandBase(long orderId, DateTime timestamp)
    {
        OrderId = orderId;
        Timestamp = timestamp;
    }

    public virtual void Fill(ref OrderCommand command)
    {
        Reset(ref command);
    }

    protected virtual void Reset(ref OrderCommand command)
    {
        command.OrderId = OrderId;
        command.Timestamp = Timestamp;

        command.Uid = long.MinValue;
        command.Action = OrderAction.ASK;
        command.BinaryCommandType = BinaryDataType.NONE;
        command.BinaryData = Array.Empty<byte>();
        command.Command = OrderCommandType.NOP;
        command.EngineEvent = null;
        command.Filled = long.MinValue;
        command.MarketData = null;
        command.OrderType = OrderType.GTC;
        command.Price = decimal.MinValue;
        command.ResultCode = CommandResultCode.NEW;
        command.Size = long.MinValue;
        command.Symbol = int.MinValue;
    }
}
