using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Adapters.SignalR.Extensions;

internal static class ApiCommandResultExtensions
{
    public static ApiCommandResult ToDto(this Application.Messages.ApiCommandResult apiRes)
    {
        var res = new ApiCommandResult()
        {
            ResultCode = (Contracts.Enums.CommandResultCode)apiRes.ResultCode,
            OrderId = apiRes.OrderId,
            Timestamp = apiRes.Timestamp,
            Seq = apiRes.Seq,
            Uid = apiRes.Uid,
            BinaryData = apiRes.BinaryData,
            BinaryCommandType = (Contracts.Enums.BinaryDataType)apiRes.BinaryCommandType,
        };

        return res;
    }
}
