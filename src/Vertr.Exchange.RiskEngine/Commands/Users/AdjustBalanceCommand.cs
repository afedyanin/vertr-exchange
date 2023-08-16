using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Users;
internal class AdjustBalanceCommand : UserCommand
{
    public AdjustBalanceCommand(IUserProfileService userProfileService, OrderCommand command)
        : base(userProfileService, command)
    {
    }

    public override CommandResultCode Execute()
    {
        throw new NotImplementedException();
    }


}
