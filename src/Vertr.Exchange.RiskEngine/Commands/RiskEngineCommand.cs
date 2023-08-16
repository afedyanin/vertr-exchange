using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands;

internal abstract class RiskEngineCommand
{
    protected OrderCommand OrderCommand { get; }

    protected IUserProfileService UserProfileService { get; }

    public RiskEngineCommand(
        IUserProfileService userProfileService,
        OrderCommand command)
    {
        OrderCommand = command;
        UserProfileService = userProfileService;
    }

    public abstract CommandResultCode Execute();
}

