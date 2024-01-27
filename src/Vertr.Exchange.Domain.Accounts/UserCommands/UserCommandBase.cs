using Vertr.Exchange.Domain.Common.Enums;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.Accounts.UserCommands;
internal abstract class UserCommandBase(
    OrderCommand orderCommand,
    IUserProfileProvider userProfilesRepository) : IUserCommand
{
    protected OrderCommand OrderCommand { get; } = orderCommand;

    protected IUserProfileProvider UserProfilesRepository { get; } = userProfilesRepository;

    protected IUserProfile? UserProfile => UserProfilesRepository.Get(OrderCommand.Uid);

    public abstract CommandResultCode Execute();
}
