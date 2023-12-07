using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Server.Extensions;

internal static class ApiCommandResultExtensions
{
    public static CommandResult ToDto(this IApiCommandResult apiRes)
    {
        var res = new CommandResult()
        {
            CommandResultCode = apiRes.ResultCode.ToDto(),
            OrderId = apiRes.OrderId,
            Timestamp = apiRes.Timestamp,
        };

        if (apiRes.MarketData != null)
        {
            res.MarketData = apiRes.MarketData.ToDto(apiRes.Timestamp);
        }

        if (apiRes.RootEvent != null)
        {
            res.Events = apiRes.RootEvent.ToDto().ToArray();
        }

        return res;
    }

    public static ApiCommandResult ToDto(this Common.Messages.ApiCommandResult apiRes)
    {
        var res = new ApiCommandResult()
        {
            ResultCode = apiRes.ResultCode.ToDto(),
            OrderId = apiRes.OrderId,
            Timestamp = apiRes.Timestamp,
            Seq = apiRes.Seq,
            Uid = apiRes.Uid,
        };

        return res;
    }
}
