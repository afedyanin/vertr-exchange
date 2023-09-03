using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Binary.Reports;
public sealed class SingleUserReportResult
{
    public long Uid { get; set; }

    public QueryExecutionStatus ExecutionStatus { get; set; }

    public UserStatus UserStatus { get; set; }

    // Symbol
    public IDictionary<int, IOrder[]> Orders { get; set; } = new Dictionary<int, IOrder[]>();

    // Currency
    public IDictionary<int, decimal> Accounts { get; set; } = new Dictionary<int, decimal>();

    // Symbol
    public IDictionary<int, IPosition> Positions { get; set; } = new Dictionary<int, IPosition>();
}
