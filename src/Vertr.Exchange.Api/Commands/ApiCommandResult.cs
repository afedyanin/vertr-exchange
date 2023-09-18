using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public record class ApiCommandResult
{
    public CommandResultCode ResultCode { get; set; }
}
