using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;

internal class ResumeUserCommand : RiskEngineCommand
{
    public ResumeUserCommand(IUserProfileService userProfileService, OrderCommand command)
        : base(userProfileService, command)
    {
    }

    public override CommandResultCode Execute()
    {
        return UserProfileService.ResumeUserProfile(OrderCommand.Uid);
    }
}
