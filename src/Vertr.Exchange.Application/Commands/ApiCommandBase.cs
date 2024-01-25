using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Application.Commands;

public abstract class ApiCommandBase(
    long orderId,
    DateTime timestamp) : IApiCommand
{
    public long OrderId { get; } = orderId;

    public DateTime Timestamp { get; } = timestamp;

    public abstract OrderCommandType CommandType { get; }

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
        command.BinaryData = [];
        command.EngineEvent = null;
        command.MarketData = null;
        command.OrderType = OrderType.GTC;
        command.Price = decimal.MinValue;
        command.Size = long.MinValue;
        command.Symbol = int.MinValue;
    }
}
