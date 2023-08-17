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
    private readonly bool _marginTradingEnabled;
    private readonly IDictionary<int, LastPriceCacheRecord> _lastPriceCache;

    public PreProcessOrderCommand(
        IUserProfileService userProfileService,
        ISymbolSpecificationProvider symbolSpecificationProvider,
        IDictionary<int, LastPriceCacheRecord> lastPriceCache,
        OrderCommand command,
        bool ignoreRiskProcessing,
        bool marginTradingEnabled)
        : base(userProfileService, command)
    {
        _symbolSpecificationProvider = symbolSpecificationProvider;
        _ignoreRiskProcessing = ignoreRiskProcessing;
        _marginTradingEnabled = marginTradingEnabled;
        _lastPriceCache = lastPriceCache;
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
        if (spec.Type == SymbolType.CURRENCY_EXCHANGE_PAIR)
        {
            return PlaceExchangeOrder(cmd, userProfile, spec);
        }
        else if (spec.Type == SymbolType.FUTURES_CONTRACT)
        {
            if (!_marginTradingEnabled)
            {
                return CommandResultCode.RISK_MARGIN_TRADING_DISABLED;
            }

            return PlaceMarginOrder(cmd, userProfile, spec);
        }
        else
        {
            return CommandResultCode.UNSUPPORTED_SYMBOL_TYPE;
        }
    }
    private CommandResultCode PlaceExchangeOrder(
        OrderCommand cmd,
        UserProfile userProfile,
        CoreSymbolSpecification spec)
    {

        var currency = cmd.Action == OrderAction.BID ? spec.QuoteCurrency : spec.BaseCurrency;

        // futures positions check for this currency
        // long freeFuturesMargin = 0L;

        if (_marginTradingEnabled)
        {
            var position = userProfile.GetPositionByCurrency(currency);

            if (position != null)
            {
                int recSymbol = position.Symbol;
                var spec2 = _symbolSpecificationProvider.GetSymbolSpecification(recSymbol);

                // add P&L subtract margin
                freeFuturesMargin += (position.EstimateProfit(spec2, _lastPriceCache.get(recSymbol)) - position.CalculateRequiredMarginForFutures(spec2));
            }
        }

        var size = cmd.Size;
        decimal orderHoldAmount;


        if (cmd.Action == OrderAction.BID)
        {
            if (cmd.OrderType is OrderType.FOK_BUDGET or OrderType.IOC_BUDGET)
            {

                if (cmd.ReserveBidPrice != cmd.Price)
                {
                    //log.warn("reserveBidPrice={} less than price={}", cmd.reserveBidPrice, cmd.price);
                    return CommandResultCode.RISK_INVALID_RESERVE_BID_PRICE;
                }

                orderHoldAmount = CoreArithmeticUtils.CalculateAmountBidTakerFeeForBudget(size, cmd.Price, spec);
                // if (logDebug) log.debug("hold amount budget buy {} = {} * {} + {} * {}", cmd.price, size, spec.quoteScaleK, size, spec.takerFee);
            }
            else
            {

                if (cmd.ReserveBidPrice < cmd.Price)
                {
                    //log.warn("reserveBidPrice={} less than price={}", cmd.reserveBidPrice, cmd.price);
                    return CommandResultCode.RISK_INVALID_RESERVE_BID_PRICE;
                }
                orderHoldAmount = CoreArithmeticUtils.CalculateAmountBidTakerFee(size, cmd.ReserveBidPrice, spec);
                // if (logDebug) log.debug("hold amount buy {} = {} * ( {} * {} + {} )", orderHoldAmount, size, cmd.reserveBidPrice, spec.quoteScaleK, spec.takerFee);
            }
        }
        else
        {
            if (cmd.Price * spec.QuoteScaleK < spec.TakerFee)
            {
                // log.debug("cmd.price {} * spec.quoteScaleK {} < {} spec.takerFee", cmd.price, spec.quoteScaleK, spec.takerFee);
                // todo also check for move command
                return CommandResultCode.RISK_ASK_PRICE_LOWER_THAN_FEE;
            }

            orderHoldAmount = CoreArithmeticUtils.CalculateAmountAsk(size, spec);
            // if (logDebug) log.debug("hold sell {} = {} * {} ", orderHoldAmount, size, spec.baseScaleK);
        }

        /*
        if (logDebug)
        {
            log.debug("R1 uid={} : orderHoldAmount={} vs serProfile.accounts.get({})={} + freeFuturesMargin={}",
                    userProfile.uid, orderHoldAmount, currency, userProfile.accounts.get(currency), freeFuturesMargin);
        }
        */

        // speculative change balance
        var newBalance = userProfile.AddToValue(currency, -orderHoldAmount);

        var canPlace = newBalance + freeFuturesMargin >= 0;

        if (!canPlace)
        {
            // revert balance change
            userProfile.AddToValue(currency, orderHoldAmount);
            //            log.warn("orderAmount={} > userProfile.accounts.get({})={}", orderAmount, currency, userProfile.accounts.get(currency));
            return CommandResultCode.RISK_NSF;
        }
        else
        {
            return CommandResultCode.VALID_FOR_MATCHING_ENGINE;
        }

    }

    private CommandResultCode PlaceMarginOrder(
        OrderCommand cmd,
        UserProfile userProfile,
        CoreSymbolSpecification spec)
    {
        var position = userProfile.GetCurrentPosition(spec.SymbolId);
        position ??= new SymbolPositionRecord(cmd.Uid, spec.SymbolId, spec.QuoteCurrency);

        var canPlaceOrder = CanPlaceMarginOrder(cmd, userProfile, spec, position);

        if (canPlaceOrder)
        {
            // position.pendingHold(cmd.action, cmd.size);
            return CommandResultCode.VALID_FOR_MATCHING_ENGINE;
        }
        else
        {
            // try to cleanup position if refusing to place
            if (position.IsEmpty())
            {
                userProfile.RemovePositionRecord(position);
            }

            return CommandResultCode.RISK_NSF;
        }
    }

    private bool CanPlaceMarginOrder(
        OrderCommand cmd,
        UserProfile userProfile,
        CoreSymbolSpecification spec,
        SymbolPositionRecord position)
    {
        var newRequiredMarginForSymbol = position.CalculateRequiredMarginForOrder(spec, cmd.Action!.Value, cmd.Size);

        if (newRequiredMarginForSymbol == -1)
        {
            // always allow placing a new order if it would not increase exposure
            return true;
        }

        // extra margin is required

        int symbol = cmd.Symbol;
        // calculate free margin for all positions same currency

        decimal freeMargin = decimal.Zero;

        foreach (var positionRecord in userProfile.Positions)
        {
            int recSymbol = positionRecord.Symbol;

            if (recSymbol != symbol)
            {
                if (positionRecord.Currency == spec.QuoteCurrency)
                {
                    var spec2 = _symbolSpecificationProvider.GetSymbolSpecification(recSymbol);

                    // add P&L subtract margin
                    _lastPriceCache.TryGetValue(recSymbol, out var cachedPrice);
                    freeMargin += positionRecord.EstimateProfit(spec2!, cachedPrice);
                    freeMargin -= positionRecord.CalculateRequiredMarginForFutures(spec2!);
                }
            }
            else
            {
                _lastPriceCache.TryGetValue(spec.SymbolId, out var cachedPrice);
                freeMargin = position.EstimateProfit(spec, cachedPrice);
            }
        }

        var currentAmount = userProfile.GetCurrentAmount(position.Currency);
        return newRequiredMarginForSymbol <= currentAmount + freeMargin;
    }
}
