using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;

internal static class UserCommandFactory
{
    public static RiskEngineCommand CreateUserCommand(
        IUserProfileService userProfileService,
        OrderRiskEngine orderRiskEngine,
        OrderCommand orderCommand)
    {
#pragma warning disable IDE0072 // Add missing cases
        return orderCommand.Command switch
        {
            OrderCommandType.ADD_USER => new AddUserCommand(userProfileService, orderCommand),
            OrderCommandType.BALANCE_ADJUSTMENT => new AdjustBalanceCommand(userProfileService, orderRiskEngine, orderCommand),
            OrderCommandType.SUSPEND_USER => new SuspendUserCommand(userProfileService, orderCommand),
            OrderCommandType.RESUME_USER => new ResumeUserCommand(userProfileService, orderCommand),
            _ => throw new InvalidOperationException("Risk engine unsupported command."),
        };
#pragma warning restore IDE0072 // Add missing cases
    }
}
