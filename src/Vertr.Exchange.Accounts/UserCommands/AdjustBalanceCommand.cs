using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Accounts.UserCommands;
internal class AdjustBalanceCommand(
    OrderCommand command,
    IUserProfileProvider userProfilesRepository)
    : UserCommandBase(command, userProfilesRepository)
{
    public override CommandResultCode Execute()
    {
        if (UserProfile is null)
        {
            return CommandResultCode.USER_MGMT_USER_NOT_FOUND;
        }

        var currency = OrderCommand.Symbol;
        // TODO: Check if suspended?
        UserProfile.AddToValue(currency, OrderCommand.Price); // Should be currency

        return CommandResultCode.SUCCESS;
    }
}
