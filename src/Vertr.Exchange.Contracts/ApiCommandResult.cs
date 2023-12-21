using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Contracts;
public record ApiCommandResult
{
    public long OrderId { get; set; }

    public long Uid { get; set; }

    public CommandResultCode ResultCode { get; set; }

    public DateTime Timestamp { get; set; }

    public long Seq { get; set; }

    public byte[] BinaryData { get; set; } = [];

    public BinaryDataType BinaryCommandType { get; set; }
}
