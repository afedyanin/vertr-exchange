using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.RiskEngine.Adjustments;

namespace Vertr.Exchange.RiskEngine.Users.UserCommands;
internal class AdjustBalanceCommand : RiskEngineCommand
{
    public AdjustBalanceCommand(
        IOrderRiskEngineInternal orderRiskEngine,
        OrderCommand command)
        : base(orderRiskEngine, command)
    {
    }

    public override CommandResultCode Execute()
    {
        var res = UserProfileService.BalanceAdjustment(
            OrderCommand.Uid,
            OrderCommand.Symbol,
            OrderCommand.Price,
            OrderCommand.OrderId,
            (BalanceAdjustmentType)OrderCommand.OrderType);

        return res;
    }
}
