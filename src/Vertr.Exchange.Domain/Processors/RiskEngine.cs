using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain.Processors;

internal class RiskEngine : IRiskEngine
{
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
                    cmd.resultCode = userProfileService.addEmptyUserProfile(cmd.uid)
                            ? CommandResultCode.SUCCESS
                            : CommandResultCode.USER_MGMT_USER_ALREADY_EXISTS;
                }
                return false;

            case OrderCommandType.BALANCE_ADJUSTMENT:
                if (uidForThisHandler(cmd.uid))
                {
                    cmd.resultCode = adjustBalance(
                            cmd.uid, cmd.symbol, cmd.price, cmd.orderId, BalanceAdjustmentType.of(cmd.orderType.getCode()));
                }
                return false;

            case OrderCommandType.SUSPEND_USER:
                if (uidForThisHandler(cmd.uid))
                {
                    cmd.resultCode = userProfileService.suspendUserProfile(cmd.uid);
                }
                return false;
            case OrderCommandType.RESUME_USER:
                if (uidForThisHandler(cmd.uid))
                {
                    cmd.resultCode = userProfileService.resumeUserProfile(cmd.uid);
                }
                return false;

            case OrderCommandType.BINARY_DATA_COMMAND:
            case OrderCommandType.BINARY_DATA_QUERY:
                binaryCommandsProcessor.acceptBinaryFrame(cmd); // ignore return result, because it should be set by MatchingEngineRouter
                if (shardId == 0)
                {
                    cmd.resultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE;
                }
                return false;

            case OrderCommandType.RESET:
                reset();
                if (shardId == 0)
                {
                    cmd.resultCode = CommandResultCode.SUCCESS;
                }
                return false;

            case OrderCommandType.PERSIST_STATE_MATCHING:
                if (shardId == 0)
                {
                    cmd.resultCode = CommandResultCode.VALID_FOR_MATCHING_ENGINE;
                }
                return true;// true = publish sequence before finishing processing whole batch

            case OrderCommandType.PERSIST_STATE_RISK:
                final boolean isSuccess = serializationProcessor.storeData(
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
        final int symbol = cmd.symbol;

        final L2MarketData marketData = cmd.marketData;
        MatcherTradeEvent mte = cmd.matcherEvent;

        // skip events processing if no events (or if contains BINARY EVENT)
        if (marketData == null && (mte == null || mte.eventType == MatcherEventType.BINARY_EVENT))
        {
            return false;
        }

        final CoreSymbolSpecification spec = symbolSpecificationProvider.getSymbolSpecification(symbol);
        if (spec == null)
        {
            throw new IllegalStateException("Symbol not found: " + symbol);
        }

        final boolean takerSell = cmd.action == OrderAction.ASK;

        if (mte != null && mte.eventType != MatcherEventType.BINARY_EVENT)
        {
            // at least one event to process, resolving primary/taker user profile
            // TODO processing order is reversed
            if (spec.type == SymbolType.CURRENCY_EXCHANGE_PAIR)
            {

                final UserProfile takerUp = uidForThisHandler(cmd.uid)
                        ? userProfileService.getUserProfileOrAddSuspended(cmd.uid)
                        : null;

                // REJECT always comes first; REDUCE is always single event
                if (mte.eventType == MatcherEventType.REDUCE || mte.eventType == MatcherEventType.REJECT)
                {
                    if (takerUp != null)
                    {
                        handleMatcherRejectReduceEventExchange(cmd, mte, spec, takerSell, takerUp);
                    }
                    mte = mte.nextEvent;
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

                final UserProfile takerUp = uidForThisHandler(cmd.uid) ? userProfileService.getUserProfileOrAddSuspended(cmd.uid) : null;

                // for margin-mode symbols also resolve position record
                final SymbolPositionRecord takerSpr = (takerUp != null) ? takerUp.getPositionRecordOrThrowEx(symbol) : null;
                do
                {
                    handleMatcherEventMargin(mte, spec, cmd.action, takerUp, takerSpr);
                    mte = mte.nextEvent;
                } while (mte != null);
            }
        }

        // Process marked data
        if (marketData != null && cfgMarginTradingEnabled)
        {
            final RiskEngine.LastPriceCacheRecord record = lastPriceCache.getIfAbsentPut(symbol, RiskEngine.LastPriceCacheRecord::new);
            record.askPrice = (marketData.askSize != 0) ? marketData.askPrices[0] : Long.MAX_VALUE;
            record.bidPrice = (marketData.bidSize != 0) ? marketData.bidPrices[0] : 0;
        }

        return false;
    }

}
