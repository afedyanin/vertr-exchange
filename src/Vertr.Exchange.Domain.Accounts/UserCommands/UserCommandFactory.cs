using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common;

namespace Vertr.Exchange.Domain.Accounts.UserCommands;

public static class UserCommandFactory
{
    public static IUserCommand CreateUserCommand(
        OrderCommand orderCommand,
        IUserProfileProvider userProfileRepository)
    {
#pragma warning disable IDE0072 // Add missing cases
        return orderCommand.Command switch
        {
            OrderCommandType.ADD_USER => new AddUserCommand(orderCommand, userProfileRepository),
            OrderCommandType.BALANCE_ADJUSTMENT => new AdjustBalanceCommand(orderCommand, userProfileRepository),
            OrderCommandType.SUSPEND_USER => new SuspendUserCommand(orderCommand, userProfileRepository),
            OrderCommandType.RESUME_USER => new ResumeUserCommand(orderCommand, userProfileRepository),
            _ => throw new InvalidOperationException($"Unsupported command - {orderCommand.Command}."),
        };
#pragma warning restore IDE0072 // Add missing cases
    }
}
