using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common.Reports.Dtos;
using Vertr.Exchange.Domain.Common.Binary.Reports;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.MatchingEngine.OrderBooks;
using Vertr.Exchange.Domain.Common.Events;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common.Reports;

namespace Vertr.Exchange.Domain.MatchingEngine.Binary;

internal static class ReportExtensions
{
    public static CommandResultCode HandleQuery(
        this SingleUserReportQuery query,
        OrderCommand command,
        IOrderBookProvider orderBookProvider)
    {
        var result = BinaryQueryFactory.GetSingleUserReportResult(command);

        result ??= new SingleUserReportResult();
        result.Uid = query.Uid;
        result.Orders = orderBookProvider.GetOrders(query.Uid).ToDto();

        EventsHelper.AttachBinaryEvent(command, result.ToBinary());

        return CommandResultCode.SUCCESS;
    }

    private static Dictionary<int, OrderDto[]> ToDto(this IDictionary<int, IOrder[]> dict)
    {
        var res = new Dictionary<int, OrderDto[]>(dict.Count);

        foreach (var key in dict.Keys)
        {
            res.Add(key, dict[key].ToOrderDto());
        }

        return res;
    }

    private static OrderDto[] ToOrderDto(this IOrder[] orders)
        => orders.Select(o => o.ToOrderDto()).ToArray();

    private static OrderDto ToOrderDto(this IOrder order)
    {
        var res = new OrderDto
        {
            Action = order.Action,
            Completed = order.Completed,
            Filled = order.Filled,
            OrderId = order.OrderId,
            Price = order.Price,
            Remaining = order.Remaining,
            Size = order.Size,
            Timestamp = order.Timestamp,
            Uid = order.Uid,
        };

        return res;
    }
}
