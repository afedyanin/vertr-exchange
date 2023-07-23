using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Enums;
using Vertr.Exchange.Domain.Users;

namespace Vertr.Exchange.Domain.Processors;

internal class RiskEngine : IRiskEngine
{
    private readonly IUserProfileService _userProfileService;

    public RiskEngine()
    {
        _userProfileService = new UserProfileService();
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
                cmd.ResultCode = placeOrderRiskCheck(cmd);
                return false;

            case OrderCommandType.ADD_USER:
                cmd.ResultCode = _userProfileService.AddEmptyUserProfile(cmd.Uid)
                        ? CommandResultCode.SUCCESS
                        : CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS;
                return false;

            case OrderCommandType.BALANCE_ADJUSTMENT:
                cmd.ResultCode = adjustBalance(
                            cmd.Uid,
                            cmd.Symbol,
                            cmd.Price,
                            cmd.OrderId,
                            BalanceAdjustmentType.of(cmd.orderType.getCode()));
                return false;

            case OrderCommandType.SUSPEND_USER:
                cmd.ResultCode = _userProfileService.SuspendUserProfile(cmd.Uid);
                return false;
            case OrderCommandType.RESUME_USER:
                cmd.ResultCode = _userProfileService.ResumeUserProfile(cmd.Uid);
                return false;

            case OrderCommandType.BINARY_DATA_COMMAND:
            case OrderCommandType.BINARY_DATA_QUERY:
                binaryCommandsProcessor.acceptBinaryFrame(cmd); // ignore return result, because it should be set by MatchingEngineRouter
                cmd.ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE;
                return false;

            case OrderCommandType.RESET:
                Reset();
                cmd.ResultCode = CommandResultCode.SUCCESS;
                return false;

            case OrderCommandType.PERSIST_STATE_MATCHING:
                cmd.ResultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE;
                return true;// true = publish sequence before finishing processing whole batch

            case OrderCommandType.PERSIST_STATE_RISK:
                var isSuccess = serializationProcessor.storeData(
                        cmd.orderId,
                        seq,
                        cmd.timestamp,
                        MODULE_RE,
                        shardId,
                        this);
                UnsafeUtils.setResultVolatile(cmd, isSuccess, CommandResultCode.SUCCESS, CommandResultCode.STATE_PERSIST_RISK_ENGINE_FAILED);
                return false;
        }
        return false;
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

        var spec = symbolSpecificationProvider.getSymbolSpecification(symbol) ?? throw new IllegalStateException("Symbol not found: " + symbol);

        var takerSell = cmd.Action == OrderAction.ASK;

        if (mte != null && mte.EventType != MatcherEventType.BINARY_EVENT)
        {
            // at least one event to process, resolving primary/taker user profile
            // TODO processing order is reversed
            if (spec.type == SymbolType.CURRENCY_EXCHANGE_PAIR)
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

        return false;
    }
}
