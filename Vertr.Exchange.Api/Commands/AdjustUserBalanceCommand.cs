using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public record class AdjustUserBalanceCommand : ApiCommand
{
    public long Uid { get; set; }

    public int Currency { get; set; }

    public decimal Amount { get; set; }

    public long TransactionId { get; set; }

    public BalanceAdjustmentType AdjustmentType => BalanceAdjustmentType.ADJUSTMENT;
}
