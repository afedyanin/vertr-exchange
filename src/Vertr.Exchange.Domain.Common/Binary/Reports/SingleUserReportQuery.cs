using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.Common.Binary.Reports;

public sealed class SingleUserReportQuery : IBinaryQuery
{
    public long Uid { get; set; }

    public ReportType ReportType => ReportType.SINGLE_USER_REPORT;
}
