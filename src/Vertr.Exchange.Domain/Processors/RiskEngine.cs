using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Binary;
using Vertr.Exchange.Domain.Enums;
using Vertr.Exchange.Domain.Symbols;
using Vertr.Exchange.Domain.Users;

namespace Vertr.Exchange.Domain.Processors;

internal class RiskEngine : IRiskEngine
{
    private const bool _cfgMarginTradingEnabled = true;
    private const bool _cfgIgnoreRiskProcessing = false;

    private readonly IUserProfileService _userProfileService;
    private readonly ISymbolSpecificationProvider _symbolSpecificationProvider;
    private readonly BinaryCommandProcessor _binaryCommandsProcessor;

    public RiskEngine()
    {
        _userProfileService = new UserProfileService();
        _symbolSpecificationProvider = new SymbolSpecificationProvider();
        _binaryCommandsProcessor = new BinaryCommandProcessor(HandleBinaryCommand);
    }

    public bool PreProcessCommand(long seq, OrderCommand cmd)
    {
        switch (cmd.Command)
        {
            case OrderCommandType.MOVE_ORDER:
            case OrderCommandType.CANCEL_ORDER:
            case OrderCommandType.REDUCE_ORDER:
            case OrderCommandType.ORDER_BOOK_REQUEST:
                return false;

            case OrderCommandType.PLACE_ORDER:
                cmd.ResultCode = PlaceOrderRiskCheck(cmd);
                return false;

            case OrderCommandType.ADD_USER:
                cmd.ResultCode = _userProfileService.AddEmptyUserProfile(cmd.Uid)
                        ? CommandResultCode.SUCCESS
                        : CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS;
                return false;

            case OrderCommandType.BALANCE_ADJUSTMENT:
                cmd.ResultCode = AdjustBalance(
                            cmd.Uid,
                            cmd.Symbol,
                            cmd.Price,
                            cmd.OrderId,
                            (BalanceAdjustmentType)cmd.OrderType);
                return false;

            case OrderCommandType.SUSPEND_USER:
                cmd.ResultCode = _userProfileService.SuspendUserProfile(cmd.Uid);
                return false;
            case OrderCommandType.RESUME_USER:
                cmd.ResultCode = _userProfileService.ResumeUserProfile(cmd.Uid);
                return false;

            case OrderCommandType.BINARY_DATA_COMMAND:
            case OrderCommandType.BINARY_DATA_QUERY:
                // ignore return result, because it should be set by MatchingEngineRouter
                var _ = _binaryCommandsProcessor.AcceptBinaryCommand(cmd);
                cmd.ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE;
                return false;

            case OrderCommandType.RESET:
                Reset();
                cmd.ResultCode = CommandResultCode.SUCCESS;
                return false;

            case OrderCommandType.PERSIST_STATE_MATCHING:
                cmd.ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE;
                return true;// true = publish sequence before finishing processing whole batch
        }
        return false;
    }

    private CommandResultCode PlaceOrderRiskCheck(OrderCommand cmd)
    {

        var userProfile = _userProfileService.GetUserProfile(cmd.Uid);
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

        if (_cfgIgnoreRiskProcessing)
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

    public bool HandlerRiskRelease(long seq, OrderCommand cmd)
    {
        var symbol = cmd.Symbol;
        var marketData = cmd.MarketData;
        var mte = cmd.MatcherEvent;

        // skip events processing if no events (or if contains BINARY EVENT)
        if (marketData == null && (mte == null || mte.EventType == MatcherEventType.BINARY_EVENT))
        {
            return false;
        }

        var spec = _symbolSpecificationProvider.GetSymbolSpecification(symbol) ??
            throw new InvalidOperationException("Symbol not found: " + symbol);

        var takerSell = cmd.Action == OrderAction.ASK;
        /*
        if (mte != null && mte.EventType != MatcherEventType.BINARY_EVENT)
        {
            // at least one event to process, resolving primary/taker user profile
            // TODO processing order is reversed
            if (spec.Type == SymbolType.CURRENCY_EXCHANGE_PAIR)
            {
                var takerUp = _userProfileService.GetUserProfileOrAddSuspended(cmd.Uid);

                // REJECT always comes first; REDUCE is always single event
                if (mte.EventType == MatcherEventType.REDUCE || mte.EventType == MatcherEventType.REJECT)
                {
                    if (takerUp != null)
                    {
                        handleMatcherRejectReduceEventExchange(cmd, mte, spec, takerSell, takerUp);
                    }
                    mte = mte.NextEvent;
                }

                if (mte != null)
                {
                    if (takerSell)
                    {
                        handleMatcherEventsExchangeSell(mte, spec, takerUp);
                    }
                    else
                    {
                        handleMatcherEventsExchangeBuy(mte, spec, takerUp, cmd);
                    }
                }
            }
            else
            {

                var takerUp = _userProfileService.GetUserProfileOrAddSuspended(cmd.Uid);

                // for margin-mode symbols also resolve position record
                var takerSpr = (takerUp != null) ? takerUp.getPositionRecordOrThrowEx(symbol) : null;
                do
                {
                    handleMatcherEventMargin(mte, spec, cmd.Action, takerUp, takerSpr);
                    mte = mte.NextEvent;
                } while (mte != null);
            }
        }

        // Process marked data
        if (marketData != null && cfgMarginTradingEnabled)
        {
            RiskEngine.LastPriceCacheRecord record = lastPriceCache.getIfAbsentPut(symbol, RiskEngine.LastPriceCacheRecord::new);
            record.askPrice = (marketData.AskSize != 0) ? marketData.AskPrices[0] : long.MaxValue;
            record.bidPrice = (marketData.BidSize != 0) ? marketData.BidPrices[0] : 0;
        }
        */
        return false;
    }

    private void HandleBinaryCommand(OrderCommand cmd, BinaryCommand binCmd)
    {
        if (binCmd is BatchAddSymbolsCommand batchAddSymbolsCommand)
        {
            var symbols = batchAddSymbolsCommand.Symbols;

            foreach (var spec in symbols)
            {
                if (spec.Type == SymbolType.CURRENCY_EXCHANGE_PAIR || _cfgMarginTradingEnabled)
                {
                    _symbolSpecificationProvider.AddSymbol(spec);
                }
                else
                {
                    // log.warn("Margin symbols are not allowed: {}", spec);
                }
            }
        }
        else if (binCmd is BatchAddAccountsCommand batchAddAccountsCommand)
        {
            var users = batchAddAccountsCommand.Users;

            foreach (var (uid, acounts) in users)
            {
                if (_userProfileService.AddEmptyUserProfile(uid))
                {
                    foreach (var (cur, bal) in acounts)
                    {
                        AdjustBalance(uid, cur, bal, 1_000_000_000 + cur, BalanceAdjustmentType.ADJUSTMENT);
                    }
                }
                else
                {
                    // log.debug("User already exist: {}", uid);
                }
            }
        }
    }

    private CommandResultCode AdjustBalance(
        long uid,
        int symbol,
        long amountDiff,
        long fundingTransactionId,
        BalanceAdjustmentType adjustmentType)
    {
        var res = _userProfileService.BalanceAdjustment(uid, symbol, amountDiff, fundingTransactionId);
        /* How to use it?
        if (res == CommandResultCode.SUCCESS)
        {
            switch (adjustmentType)
            {
                case BalanceAdjustmentType.ADJUSTMENT:
                    adjustments.addToValue(symbol, -amountDiff);
                    break;

                case BalanceAdjustmentType.SUSPEND:
                    suspends.addToValue(symbol, -amountDiff);
                    break;
            }
        }
        */
        return res;
    }

    public void Reset()
    {
        _userProfileService.Reset();
        _symbolSpecificationProvider.Reset();
        // lastPriceCache.clear();
        // fees.clear();
        // adjustments.clear();
        // suspends.clear();
    }
}
