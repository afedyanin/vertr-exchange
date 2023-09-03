using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts.UserCommands;
internal class AddUserCommand : UserCommandBase
{
    public AddUserCommand(
        OrderCommand command,
        IUserProfileProvider userProfilesRepository)
        : base(command, userProfilesRepository)
    {
    }

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
