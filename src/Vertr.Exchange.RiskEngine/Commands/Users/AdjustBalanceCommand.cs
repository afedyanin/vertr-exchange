using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;
internal class AdjustBalanceCommand : RiskEngineCommand
{
    public AdjustBalanceCommand(IUserProfileService userProfileService, OrderCommand command)
        : base(userProfileService, command)
    {
    }

    public override CommandResultCode Execute()
    {
        var res = UserProfileService.BalanceAdjustment(
            OrderCommand.Uid,
            OrderCommand.Symbol,
            OrderCommand.Price,
            OrderCommand.OrderId);

        return res;

        /*
        if (res == CommandResultCode.SUCCESS)
        {
            // (BalanceAdjustmentType)OrderCommand.OrderType
            switch (adjustmentType)
            {
                case BalanceAdjustmentType.ADJUSTMENT:
                    //adjustments.addToValue(symbol, -amountDiff);
                    break;

                case BalanceAdjustmentType.SUSPEND:
                    //suspends.addToValue(symbol, -amountDiff);
                    break;
                default:
                    break;
            }
        }
        */

    }
}
