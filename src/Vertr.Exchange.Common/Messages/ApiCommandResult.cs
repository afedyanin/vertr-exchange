using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Messages;
public record class ApiCommandResult
{
    public long OrderId { get; init; }

    public long Uid { get; init; }

    public CommandResultCode ResultCode { get; init; }

    public DateTime Timestamp { get; init; }

    public long Seq { get; init; }

    public byte[] BinaryData { get; init; } = [];

    public BinaryDataType BinaryCommandType { get; init; }
}
