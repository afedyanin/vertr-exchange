using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Accounts.UserCommands;
internal class AddUserCommand(
    OrderCommand command,
    IUserProfileProvider userProfilesRepository)
    : UserCommandBase(command, userProfilesRepository)
{
    public override CommandResultCode Execute()
    {
        if (UserProfile is not null)
        {
            return CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS;
        }

        UserProfilesRepository.GetOrAdd(OrderCommand.Uid, UserStatus.ACTIVE);
        return CommandResultCode.SUCCESS;
    }
}
