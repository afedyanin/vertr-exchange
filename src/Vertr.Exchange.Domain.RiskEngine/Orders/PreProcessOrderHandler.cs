using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Domain.RiskEngine.Symbols;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common;

namespace Vertr.Exchange.Domain.RiskEngine.Orders;

internal class PreProcessOrderHandler(
    IUserProfileProvider userProfiles,
    ISymbolSpecificationProvider symbols)
{
    private readonly IUserProfileProvider _userProfiles = userProfiles;
    private readonly ISymbolSpecificationProvider _symbols = symbols;

    public CommandResultCode Handle(OrderCommand orderCommand)
    {
        var userProfile = _userProfiles.Get(orderCommand.Uid);

        if (userProfile == null)
        {
            orderCommand.ResultCode = CommandResultCode.AUTH_INVALID_USER;
            // log.warn("User profile {} not found", cmd.uid);
            return CommandResultCode.AUTH_INVALID_USER;
        }

        var spec = _symbols.GetSymbol(orderCommand.Symbol);

        if (spec == null)
        {
            // log.warn("Symbol {} not found", cmd.symbol);
            return CommandResultCode.INVALID_SYMBOL;
        }

        return CommandResultCode.VALID_FOR_MATCHING_ENGINE;
    }
}
