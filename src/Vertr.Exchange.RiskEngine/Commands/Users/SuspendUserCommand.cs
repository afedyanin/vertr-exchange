using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;
internal class SuspendUserCommand : UserCommand
{
    public SuspendUserCommand(IUserProfileService userProfileService, OrderCommand command)
        : base(userProfileService, command)
    {
    }

    public override CommandResultCode Execute()
    {
        return UserProfileService.SuspendUserProfile(OrderCommand.Uid);
    }
}
