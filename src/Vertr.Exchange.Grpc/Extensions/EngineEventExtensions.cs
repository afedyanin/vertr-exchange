using Google.Protobuf;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Grpc.Extensions;

internal static class EngineEventExtensions
{
    public static IEnumerable<ExchangeEvent> ToGrpc(this IEngineEvent rootEvent)
    {
        var res = new List<ExchangeEvent>();
        var current = rootEvent;

        while (current != null)
        {
            res.Add(current.ToGrpcSingle());
            current = current.NextEvent;
        }

        return res;
    }

    private static ExchangeEvent ToGrpcSingle(this IEngineEvent evt)
    {
        var res = new ExchangeEvent()
        {
            ActiveOrderCompleted = evt.ActiveOrderCompleted,
            MatchedOrderCompleted = evt.MatchedOrderCompleted,
            MatchedOrderId = evt.MatchedOrderId,
            MatchedOrderUid = evt.MatchedOrderUid,
            Price = evt.Price,
            Size = evt.Size,
            BinaryData = ByteString.CopyFrom(evt.BinaryData),
            EventType = evt.EventType.ToGrpc(),
        };

        return res;
    }
}
