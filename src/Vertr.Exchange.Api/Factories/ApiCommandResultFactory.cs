using Vertr.Exchange.Api.Commands;
using Vertr.Exchange.Common;

namespace Vertr.Exchange.Api.Factories;
internal static class ApiCommandResultFactory
{
    public static ApiCommandResult CreateResult(OrderCommand command)
    {
        return new ApiCommandResult()
        {
            ResultCode = command.ResultCode,
        };
    }
}
