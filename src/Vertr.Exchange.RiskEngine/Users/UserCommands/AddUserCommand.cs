using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Users.UserCommands;
internal class AddUserCommand : RiskEngineCommand
{
    public AddUserCommand(IOrderRiskEngineInternal orderRiskEngine, OrderCommand command)
        : base(orderRiskEngine, command)
    {
    }

    public override CommandResultCode Execute()
    {
        var code = UserProfileService.AddEmptyUserProfile(OrderCommand.Uid)
                ? CommandResultCode.SUCCESS
                : CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS;

        return code;
    }
}
