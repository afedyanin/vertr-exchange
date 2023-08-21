using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Users.UserCommands;

internal class ResumeUserCommand : RiskEngineCommand
{
    public ResumeUserCommand(
        IOrderRiskEngineInternal orderRiskEngine,
        OrderCommand command)
        : base(orderRiskEngine, command)
    {
    }

    public override CommandResultCode Execute()
    {
        return UserProfileService.ResumeUserProfile(OrderCommand.Uid);
    }
}
