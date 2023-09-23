using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common;
using Vertr.Exchange.RiskEngine.Symbols;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.RiskEngine.Orders;

internal class PreProcessOrderHandler
{
    private readonly IUserProfileProvider _userProfiles;
    private readonly ISymbolSpecificationProvider _symbols;

    public PreProcessOrderHandler(
        IUserProfileProvider userProfiles,
        ISymbolSpecificationProvider symbols)
    {
        _userProfiles = userProfiles;
        _symbols = symbols;
    }

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
