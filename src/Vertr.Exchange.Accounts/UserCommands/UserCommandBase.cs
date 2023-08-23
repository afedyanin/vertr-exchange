using Vertr.Exchange.Accounts.Abstractions;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts.UserCommands;
internal abstract class UserCommandBase : IUserCommand
{
    protected OrderCommand OrderCommand { get; }

    protected IUserProfilesRepository UserProfilesRepository { get; }

    protected IUserProfile? UserProfile => UserProfilesRepository.Get(OrderCommand.Uid);

    public UserCommandBase(
        OrderCommand orderCommand,
        IUserProfilesRepository userProfilesRepository)
    {
        OrderCommand = orderCommand;
        UserProfilesRepository = userProfilesRepository;
    }

    public abstract CommandResultCode Execute();
}
