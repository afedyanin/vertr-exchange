using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Binary.Reports;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Common.Events;
using Vertr.Exchange.Shared.Reports.Dtos;
using Vertr.Exchange.Shared.Reports;

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
            result.Positions = userProfile.Positions.ToDto();
            result.ExecutionStatus = QueryExecutionStatus.OK;
        }

        EventsHelper.AttachBinaryEvent(command, result.ToBinary());

        return CommandResultCode.SUCCESS;
    }

    private static Dictionary<int, PositionDto> ToDto(this IDictionary<int, IPosition> dict)
    {
        var res = new Dictionary<int, PositionDto>(dict.Count);

        foreach (var key in dict.Keys)
        {
            res.Add(key, dict[key].ToDto());
        }

        return res;
    }

    private static PositionDto ToDto(this IPosition pos)
    {
        return new PositionDto
        {
            Direction = pos.Direction,
            OpenVolume = pos.OpenVolume,
            RealizedPnL = pos.RealizedPnL,
            Symbol = pos.Symbol,
            Uid = pos.Uid,
        };
    }
}
