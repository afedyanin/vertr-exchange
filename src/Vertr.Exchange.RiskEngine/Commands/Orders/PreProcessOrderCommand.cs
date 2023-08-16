using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Symbols;
using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.RiskEngine.Users;

namespace Vertr.Exchange.RiskEngine.Commands.Orders;

internal class PreProcessOrderCommand : RiskEngineCommand
{
    private readonly ISymbolSpecificationProvider _symbolSpecificationProvider;
    private readonly bool _ignoreRiskProcessing;

    public PreProcessOrderCommand(
        IUserProfileService userProfileService,
        ISymbolSpecificationProvider symbolSpecificationProvider,
        OrderCommand command,
        bool ignoreRiskProcessing)
        : base(userProfileService, command)
    {
        _symbolSpecificationProvider = symbolSpecificationProvider;
        _ignoreRiskProcessing = ignoreRiskProcessing;
    }

    public override CommandResultCode Execute()
    {
        return PlaceOrderRiskCheck(OrderCommand);
    }

    private CommandResultCode PlaceOrderRiskCheck(OrderCommand cmd)
    {
        var userProfile = UserProfileService.GetUserProfile(cmd.Uid);
        if (userProfile == null)
        {
            cmd.ResultCode = CommandResultCode.AUTH_INVALID_USER;
            // log.warn("User profile {} not found", cmd.uid);
            return CommandResultCode.AUTH_INVALID_USER;
        }

        var spec = _symbolSpecificationProvider.GetSymbolSpecification(cmd.Symbol);

        if (spec == null)
        {
            // log.warn("Symbol {} not found", cmd.symbol);
            return CommandResultCode.INVALID_SYMBOL;
        }

        if (_ignoreRiskProcessing)
        {
            // skip processing
            return CommandResultCode.VALID_FOR_MATCHING_ENGINE;
        }

        // check if account has enough funds
        var resultCode = PlaceOrder(cmd, userProfile, spec);

        if (resultCode != CommandResultCode.VALID_FOR_MATCHING_ENGINE)
        {
            // log.warn("{} risk result={} uid={}: Can not place {}", cmd.orderId, resultCode, userProfile.uid, cmd);
            // log.warn("{} accounts:{}", cmd.orderId, userProfile.accounts);
            return CommandResultCode.RISK_NSF;
        }

        return resultCode;
    }

    private CommandResultCode PlaceOrder(
        OrderCommand cmd,
        UserProfile userProfile,
        CoreSymbolSpecification spec)
    {
        throw new NotImplementedException();
    }

}
