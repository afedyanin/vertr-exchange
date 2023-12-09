using Vertr.Exchange.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Api.Commands;
public class AdjustBalanceCommand(
    long orderId,
    DateTime timestamp,
    long uid,
    int currency,
    decimal amount)
    : ApiCommandBase(orderId, timestamp)
{
    public override OrderCommandType CommandType => OrderCommandType.BALANCE_ADJUSTMENT;

    public long Uid { get; } = uid;

    public int Currency { get; } = currency;

    public decimal Amount { get; } = amount;

    public BalanceAdjustmentType AdjustmentType => BalanceAdjustmentType.ADJUSTMENT;

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Currency; // !
        command.Price = Amount; // !
        command.OrderType = (OrderType)AdjustmentType; // ! never used
    }
}
