using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Server.Extensions;

internal static class ApiCommandResultExtensions
{
    public static ApiCommandResult ToDto(this Common.Messages.ApiCommandResult apiRes)
    {
        var res = new ApiCommandResult()
        {
            ResultCode = apiRes.ResultCode.ToDto(),
            OrderId = apiRes.OrderId,
            Timestamp = apiRes.Timestamp,
            Seq = apiRes.Seq,
            Uid = apiRes.Uid,
            BinaryData = apiRes.BinaryData,
            BinaryCommandType = apiRes.BinaryCommandType.ToDto(),
        };

        return res;
    }
}
