using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.Accounts.UserCommands;

internal class ResumeUserCommand(
    OrderCommand command,
    IUserProfileProvider userProfilesRepository)
    : UserCommandBase(command, userProfilesRepository)
{
    public override CommandResultCode Execute()
    {
        if (UserProfile is null)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_FOUND;
        }

        return UserProfile.Resume();
    }
}
