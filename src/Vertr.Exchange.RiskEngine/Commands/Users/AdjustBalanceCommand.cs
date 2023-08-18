using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;
internal class AdjustBalanceCommand : RiskEngineCommand
{
    private readonly OrderRiskEngine _orderRiskEngine;
    public AdjustBalanceCommand(
        IUserProfileService userProfileService,
        OrderRiskEngine orderRiskEngine,
        OrderCommand command)
        : base(userProfileService, command)
    {
        _orderRiskEngine = orderRiskEngine;
    }

    public override CommandResultCode Execute()
    {
        var res = UserProfileService.BalanceAdjustment(
            OrderCommand.Uid,
            OrderCommand.Symbol,
            OrderCommand.Price,
            OrderCommand.OrderId);

        if (res == CommandResultCode.SUCCESS)
        {
            _orderRiskEngine.AdjustBalance(OrderCommand.Symbol, OrderCommand.Price, (BalanceAdjustmentType)OrderCommand.OrderType);
        }

        return res;
    }
}
