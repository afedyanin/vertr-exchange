using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.Server.Extensions;

internal static class PositionPnlRecordExtensions
{
    public static PositionPnlRecordDto[] ToDto(this PositionPnlRecord[] pnlRecords)
        => pnlRecords.Select(ToDto).ToArray();

    public static PositionPnlRecordDto ToDto(this PositionPnlRecord pnlRecord)
        => new PositionPnlRecordDto
        {
            Direction = pnlRecord.Direction,
            FixedPnL = pnlRecord.FixedPnL,
            IsEmpty = pnlRecord.IsEmpty,
            OpenPriceSum = pnlRecord.OpenPriceSum,
            OpenVolume = pnlRecord.OpenVolume,
            PnL = pnlRecord.PnL,
            Timestamp = pnlRecord.Timestamp,
        };
}
