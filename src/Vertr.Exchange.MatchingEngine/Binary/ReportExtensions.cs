using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Binary.Reports;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Events;
using Vertr.Exchange.MatchingEngine.OrderBooks;

namespace Vertr.Exchange.MatchingEngine.Binary;

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
        result.Orders = orderBookProvider.GetOrders(query.Uid);

        EventsHelper.AttachBinaryEvent(command, result.ToBinary());

        return CommandResultCode.SUCCESS;
    }
}
