using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;

internal abstract class UserCommand
{
    protected OrderCommand OrderCommand { get; }

    protected IUserProfileService UserProfileService { get; }

    public UserCommand(
        IUserProfileService userProfileService,
        OrderCommand command)
    {
        OrderCommand = command;
        UserProfileService = userProfileService;
    }

    public abstract CommandResultCode Execute();
}
