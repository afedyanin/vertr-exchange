using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;
internal class AddUserCommand : UserCommand
{
    public AddUserCommand(IUserProfileService userProfileService, OrderCommand command)
        : base(userProfileService, command)
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
