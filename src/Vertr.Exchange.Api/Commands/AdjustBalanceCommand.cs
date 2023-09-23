using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Api.Commands;
public class AdjustBalanceCommand : ApiCommandBase
{
    public override OrderCommandType CommandType => OrderCommandType.BALANCE_ADJUSTMENT;

    public long Uid { get; }

    public int Currency { get; }

    public decimal Amount { get; }

    public BalanceAdjustmentType AdjustmentType => BalanceAdjustmentType.ADJUSTMENT;

    public AdjustBalanceCommand(
        long orderId,
        DateTime timestamp,
        long uid,
        int currency,
        decimal amount) : base(orderId, timestamp)
    {
        Uid = uid;
        Currency = currency;
        Amount = amount;
    }

    public override void Fill(ref OrderCommand command)
    {
        base.Fill(ref command);

        command.Uid = Uid;
        command.Symbol = Currency; // !
        command.Price = Amount; // !
        command.OrderType = (OrderType)AdjustmentType; // ! never used
    }
}