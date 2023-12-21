using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Accounts.UserCommands;
internal abstract class UserCommandBase(
    OrderCommand orderCommand,
    IUserProfileProvider userProfilesRepository) : IUserCommand
{
    protected OrderCommand OrderCommand { get; } = orderCommand;

    protected IUserProfileProvider UserProfilesRepository { get; } = userProfilesRepository;

    protected IUserProfile? UserProfile => UserProfilesRepository.Get(OrderCommand.Uid);

    public abstract CommandResultCode Execute();
}
