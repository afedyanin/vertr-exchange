using Google.Protobuf.WellKnownTypes;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class ApiCommandResultExtensions
{
    public static CommandResult ToProto(this IApiCommandResult apiRes)
    {
        var res = new CommandResult()
        {
            CommandResultCode = apiRes.ResultCode.ToProto(),
            OrderId = apiRes.OrderId,
            Timestamp = apiRes.Timestamp.ToTimestamp(),
        };

        if (apiRes.MarketData != null)
        {
            res.MarketData = apiRes.MarketData.ToProto(apiRes.Timestamp);
        }

        if (apiRes.RootEvent != null)
        {
            res.Events.AddRange(apiRes.RootEvent.ToProto());
        }

        return res;
    }

    public static ApiCommandResult ToProto(this Common.Messages.ApiCommandResult apiRes)
    {
        var res = new ApiCommandResult()
        {
            ResultCode = apiRes.ResultCode.ToProto(),
            OrderId = apiRes.OrderId,
            Timestamp = apiRes.Timestamp.ToTimestamp(),
            Seq = apiRes.Seq,
            Uid = apiRes.Uid,
        };

        return res;
    }
}
