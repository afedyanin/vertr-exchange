using Google.Protobuf.WellKnownTypes;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Grpc.Extensions;

internal static class ApiCommandResultExtensions
{
    public static CommandResult ToGrpc(this IApiCommandResult apiRes)
    {
        var res = new CommandResult()
        {
            CommandResultCode = apiRes.ResultCode.ToGrpc(),
            OrderId = apiRes.OrderId,
            Timestamp = apiRes.Timestamp.ToTimestamp(),
        };

        if (apiRes.MarketData != null)
        {
            res.MarketData = apiRes.MarketData.ToGrpc(apiRes.Timestamp);
        }

        if (apiRes.RootEvent != null)
        {
            res.Events.AddRange(apiRes.RootEvent.ToGrpc());
        }

        return res;
    }
}
