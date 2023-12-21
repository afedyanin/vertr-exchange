using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Common.Binary.Reports;

public sealed class SingleUserReportQuery : IBinaryQuery
{
    public long Uid { get; set; }

    public ReportType ReportType => ReportType.SINGLE_USER_REPORT;
}
