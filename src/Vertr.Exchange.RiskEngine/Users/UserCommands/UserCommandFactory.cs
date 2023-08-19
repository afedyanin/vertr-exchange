using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Users.UserCommands;

internal static class UserCommandFactory
{
    public static RiskEngineCommand CreateUserCommand(
        IOrderRiskEngineInternal orderRiskEngine,
        OrderCommand orderCommand)
    {
#pragma warning disable IDE0072 // Add missing cases
        return orderCommand.Command switch
        {
            OrderCommandType.ADD_USER => new AddUserCommand(orderRiskEngine, orderCommand),
            OrderCommandType.BALANCE_ADJUSTMENT => new AdjustBalanceCommand(orderRiskEngine, orderCommand),
            OrderCommandType.SUSPEND_USER => new SuspendUserCommand(orderRiskEngine, orderCommand),
            OrderCommandType.RESUME_USER => new ResumeUserCommand(orderRiskEngine, orderCommand),
            _ => throw new InvalidOperationException("Risk engine unsupported command."),
        };
#pragma warning restore IDE0072 // Add missing cases
    }
}
