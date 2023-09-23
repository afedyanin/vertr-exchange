using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;

public abstract class ApiCommandBase : IApiCommand
{
    public long OrderId { get; }

    public DateTime Timestamp { get; }

    public abstract OrderCommandType CommandType { get; }

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
        command.Command = CommandType;
        command.OrderId = OrderId;
        command.Timestamp = Timestamp;
        command.ResultCode = CommandResultCode.NEW;

        command.Uid = long.MinValue;
        command.Action = OrderAction.ASK;
        command.BinaryCommandType = BinaryDataType.NONE;
        command.BinaryData = Array.Empty<byte>();
        command.EngineEvent = null;
        command.Filled = long.MinValue;
        command.MarketData = null;
        command.OrderType = OrderType.GTC;
        command.Price = decimal.MinValue;
        command.Size = long.MinValue;
        command.Symbol = int.MinValue;
    }
}
