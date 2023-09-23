using Vertr.Exchange.Common.Binary.Reports.Dtos;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Binary.Reports;
public sealed class SingleUserReportResult
{
    public long Uid { get; set; }

    public QueryExecutionStatus ExecutionStatus { get; set; } = QueryExecutionStatus.USER_NOT_FOUND;

    public UserStatus UserStatus { get; set; } = UserStatus.SUSPENDED;

    // Symbol
    public IDictionary<int, OrderDto[]> Orders { get; set; } = new Dictionary<int, OrderDto[]>();

    // Currency
    public IDictionary<int, decimal> Accounts { get; set; } = new Dictionary<int, decimal>();

    // Symbol
    public IDictionary<int, PositionDto> Positions { get; set; } = new Dictionary<int, PositionDto>();
}
