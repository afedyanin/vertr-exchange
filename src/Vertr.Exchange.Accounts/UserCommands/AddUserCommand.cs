using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Domain.Accounts.UserCommands;
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
