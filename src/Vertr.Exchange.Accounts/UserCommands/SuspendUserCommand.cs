using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts.UserCommands;
internal class SuspendUserCommand : UserCommandBase
{
    public SuspendUserCommand(
        OrderCommand command,
        IUserProfileProvider userProfilesRepository)
        : base(command, userProfilesRepository)
    {
    }

    public override CommandResultCode Execute()
    {
        if (UserProfile is null)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_FOUND;
        }

        return UserProfile.Suspend();
    }
}
