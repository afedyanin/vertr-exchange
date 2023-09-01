using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Reports;

internal sealed class SingleUserReportQuery
{
    public long Uid { get; set; }

    public ReportType ReportType => ReportType.SINGLE_USER_REPORT;
}
