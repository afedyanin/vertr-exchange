using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Abstractions;

public interface IApiCommandResult
{
    CommandResultCode ResultCode { get; }
}
