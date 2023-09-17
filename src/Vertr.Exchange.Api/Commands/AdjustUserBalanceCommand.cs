using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public record class AdjustUserBalanceCommand : ApiCommandBase
{
    public AdjustUserBalanceCommand(long orderId, DateTime timestamp) : base(orderId, timestamp)
    {
    }

    public long Uid { get; set; }

    public int Currency { get; set; }

    public decimal Amount { get; set; }

    public long TransactionId { get; set; }

    public BalanceAdjustmentType AdjustmentType => BalanceAdjustmentType.ADJUSTMENT;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

    }
}
