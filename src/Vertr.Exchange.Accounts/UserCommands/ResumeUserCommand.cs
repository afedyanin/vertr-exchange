using Vertr.Exchange.Accounts.Abstractions;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts.UserCommands;

internal class ResumeUserCommand : UserCommandBase
{
    public ResumeUserCommand(
        OrderCommand command,
        IUserProfilesRepository userProfilesRepository)
        : base(command, userProfilesRepository)
    {
    }

    public override CommandResultCode Execute()
    {
        if (UserProfile is null)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_FOUND;
        }

        var resumed = UserProfile.Resume();

        return resumed ? CommandResultCode.SUCCESS : CommandResultCode.USER_MGMT_USER_NOT_SUSPENDED; // TODO: Fix it
    }
}
