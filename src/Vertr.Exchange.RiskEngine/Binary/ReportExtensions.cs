using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary.Reports;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Events;

namespace Vertr.Exchange.RiskEngine.Binary;

internal static class ReportExtensions
{
    public static CommandResultCode HandleQuery(
        this SingleUserReportQuery query,
        OrderCommand command,
        IUserProfileProvider userProfiles)
    {
        var result = BinaryQueryFactory.GetSingleUserReportResult(command);
        result ??= new SingleUserReportResult();

        result.Uid = query.Uid;

        var userProfile = userProfiles.Get(command.Uid);

        if (userProfile != null)
        {
            result.UserStatus = userProfile.Status;
            result.Accounts = userProfile.Accounts;
            result.Positions = userProfile.Positions;
            result.ExecutionStatus = QueryExecutionStatus.OK;
        }

        EventsHelper.AttachBinaryEvent(command, result.ToBinary());

        return CommandResultCode.SUCCESS;
    }
}
