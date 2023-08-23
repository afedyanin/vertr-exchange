using System.Diagnostics;
using Vertr.Exchange.Accounts.Abstractions;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Symbols;
using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.RiskEngine.Extensions;

namespace Vertr.Exchange.RiskEngine.Orders;

internal class PostProcessOrderCommand : RiskEngineCommand
{
    public PostProcessOrderCommand(
        IOrderRiskEngineInternal orderRiskEngine,
        OrderCommand command) : base(orderRiskEngine, command)
    {
    }

    public override CommandResultCode Execute()
    {
        var symbol = OrderCommand.Symbol;
        var marketData = OrderCommand.MarketData;
        var matcherEvent = OrderCommand.MatcherEvent;

        // skip events processing if no events (or if contains BINARY EVENT)
        if (marketData == null && (matcherEvent == null || matcherEvent.EventType == MatcherEventType.BINARY_EVENT))
        {
            return CommandResultCode.SUCCESS;
        }

        var spec = SymbolSpecificationProvider.GetSymbolSpecification(symbol) ??
            throw new InvalidOperationException("Symbol not found: " + symbol);

        var takerSell = OrderCommand.Action == OrderAction.ASK;
        var takerProfile = UserProfiles.GetOrAdd(OrderCommand.Uid, Accounts.Enums.UserStatus.SUSPENDED);

        if (matcherEvent != null && matcherEvent.EventType != MatcherEventType.BINARY_EVENT)
        {
            // at least one event to process, resolving primary/taker user profile
            // TODO processing order is reversed
            if (spec.Type == SymbolType.CURRENCY_EXCHANGE_PAIR)
            {

                // REJECT always comes first; REDUCE is always single event
                if (matcherEvent.EventType is MatcherEventType.REDUCE or MatcherEventType.REJECT)
                {
                    HandleMatcherRejectReduceEventExchange(OrderCommand, matcherEvent, spec, takerSell, takerProfile);
                    matcherEvent = matcherEvent.NextEvent;
                }

                if (matcherEvent != null)
                {
                    if (takerSell)
                    {
                        HandleMatcherEventsExchangeSell(matcherEvent, spec, takerProfile);
                    }
                    else
                    {
                        HandleMatcherEventsExchangeBuy(matcherEvent, spec, takerProfile, OrderCommand);
                    }
                }
            }
            else
            {

                // for margin-mode symbols also resolve position record
                var takerPosition = takerProfile.GetPosition(symbol);
                do
                {
                    HandleMatcherEventMargin(matcherEvent, spec, OrderCommand.Action!.Value, takerProfile, takerSpr!);
                    matcherEvent = matcherEvent.NextEvent;
                } while (matcherEvent != null);
            }
        }

        // Process marked data
        if (marketData is not null && OrderRiskEngine.IsMarginTradingEnabled)
        {
            // TODO: Check sorting
            var askPrice = marketData.AskSize != 0 ? marketData.AskPrices[0] : long.MaxValue;
            var bidPrice = marketData.BidSize != 0 ? marketData.BidPrices[0] : 0;
            LastPriceCacheProvider.AddLastPrice(symbol, askPrice, bidPrice);
        }

        return CommandResultCode.SUCCESS;
    }

    private void HandleMatcherEventMargin(
        IMatcherTradeEvent ev,
        CoreSymbolSpecification spec,
        OrderAction takerAction,
        IUserProfile takerUp,
        Position takerSpr)
    {
        if (takerUp != null)
        {
            if (ev.EventType == MatcherEventType.TRADE)
            {
                // update taker's position
                var sizeOpen = takerSpr.UpdatePositionForMarginTrade(takerAction, ev.Size, ev.Price);
                var fee = spec.TakerFee * sizeOpen;
                takerUp.AddToValue(spec.QuoteCurrency, -fee);

                FeeCalculationService.AddFeeValue(spec.QuoteCurrency, fee);
            }
            else if (ev.EventType is MatcherEventType.REJECT or MatcherEventType.REDUCE)
            {
                // for cancel/rejection only one party is involved
                takerSpr.PendingRelease(takerAction, ev.Size);
            }

            if (takerSpr.IsEmpty())
            {
                takerUp.RemovePositionRecord(takerSpr);
            }
        }

        if (ev.EventType == MatcherEventType.TRADE)
        {
            // update maker's position
            var maker = UserProfileService.GetUserProfileOrAddSuspended(ev.MatchedOrderUid);
            var makerSpr = maker.GetCurrentPosition(spec.SymbolId);
            var sizeOpen = makerSpr!.UpdatePositionForMarginTrade(GetOppositeAction(takerAction), ev.Size, ev.Price);
            var fee = spec.MakerFee * sizeOpen;
            maker.AddToValue(spec.QuoteCurrency, -fee);

            FeeCalculationService.AddFeeValue(spec.QuoteCurrency, fee);

            if (makerSpr.IsEmpty())
            {
                maker.RemovePositionRecord(makerSpr);
            }
        }
    }

    private void HandleMatcherRejectReduceEventExchange(
        OrderCommand cmd,
        IMatcherTradeEvent ev,
        CoreSymbolSpecification spec,
        bool takerSell,
        UserProfile taker)
    {
        //log.debug("REDUCE/REJECT {} {}", cmd, ev);

        // for cancel/rejection only one party is involved
        if (takerSell)
        {
            taker.AddToValue(spec.BaseCurrency, spec.CalculateAmountAsk(ev.Size));
        }
        else
        {
            if (cmd.Command == OrderCommandType.PLACE_ORDER && cmd.OrderType == OrderType.FOK_BUDGET)
            {
                taker.AddToValue(spec.QuoteCurrency, spec.CalculateAmountBidTakerFeeForBudget(ev.Size, ev.Price));
            }
            else
            {
                taker.AddToValue(spec.QuoteCurrency, spec.CalculateAmountBidTakerFee(ev.Size, ev.BidderHoldPrice));
            }
            // TODO for OrderType.IOC_BUDGET - for REJECT should release leftover deposit after all trades calculated
        }
    }

    private void HandleMatcherEventsExchangeSell(
        IMatcherTradeEvent ev,
        CoreSymbolSpecification spec,
        UserProfile taker)
    {
        //log.debug("TRADE EXCH SELL {}", ev);

        var takerSizeForThisHandler = decimal.Zero;
        var makerSizeForThisHandler = decimal.Zero;
        var takerSizePriceForThisHandler = decimal.Zero;
        var quoteCurrency = spec.QuoteCurrency;

        while (ev != null)
        {
            Debug.Assert(ev.EventType == MatcherEventType.TRADE);

            // aggregate transfers for selling taker
            if (taker != null)
            {
                takerSizePriceForThisHandler += ev.Size * ev.Price;
                takerSizeForThisHandler += ev.Size;
            }

            var size = ev.Size;
            var maker = UserProfileService.GetUserProfileOrAddSuspended(ev.MatchedOrderUid);

            // buying, use bidderHoldPrice to calculate released amount based on price difference
            var priceDiff = ev.BidderHoldPrice - ev.Price;
            var amountDiffToReleaseInQuoteCurrency = spec.CalculateAmountBidReleaseCorrMaker(size, priceDiff);
            maker.AddToValue(quoteCurrency, amountDiffToReleaseInQuoteCurrency);

            var gainedAmountInBaseCurrency = spec.CalculateAmountAsk(size);
            maker.AddToValue(spec.BaseCurrency, gainedAmountInBaseCurrency);
            makerSizeForThisHandler += size;

            ev = ev.NextEvent!;
        }

        taker?.AddToValue(quoteCurrency, (takerSizePriceForThisHandler * spec.QuoteScaleK) - (spec.TakerFee * takerSizeForThisHandler));

        if (takerSizeForThisHandler != 0 || makerSizeForThisHandler != 0)
        {
            FeeCalculationService.AddFeeValue(quoteCurrency, (spec.TakerFee * takerSizeForThisHandler) + (spec.MakerFee * makerSizeForThisHandler));
        }
    }

    private void HandleMatcherEventsExchangeBuy(
        IMatcherTradeEvent ev,
        CoreSymbolSpecification spec,
        UserProfile taker,
        OrderCommand cmd)
    {
        //log.debug("TRADE EXCH BUY {}", ev);

        var takerSizeForThisHandler = decimal.Zero;
        var makerSizeForThisHandler = decimal.Zero;
        var takerSizePriceSum = decimal.Zero;
        var takerSizePriceHeldSum = decimal.Zero;

        var quoteCurrency = spec.QuoteCurrency;

        while (ev != null)
        {
            Debug.Assert(ev.EventType == MatcherEventType.TRADE);

            // perform transfers for taker
            if (taker != null)
            {

                takerSizePriceSum += ev.Size * ev.Price;
                takerSizePriceHeldSum += ev.Size * ev.BidderHoldPrice;
                takerSizeForThisHandler += ev.Size;
            }

            var size = ev.Size;
            var maker = UserProfileService.GetUserProfileOrAddSuspended(ev.MatchedOrderUid);
            var gainedAmountInQuoteCurrency = spec.CalculateAmountBid(size, ev.Price);
            maker.AddToValue(quoteCurrency, gainedAmountInQuoteCurrency - (spec.MakerFee * size));
            makerSizeForThisHandler += size;

            ev = ev.NextEvent!;
        }

        if (taker != null)
        {

            if (cmd.Command == OrderCommandType.PLACE_ORDER && cmd.OrderType == OrderType.FOK_BUDGET)
            {
                // for FOK budget held sum calculated differently
                takerSizePriceHeldSum = cmd.Price;
            }
            // TODO IOC_BUDGET - order can be partially rejected - need held taker fee correction

            taker.AddToValue(quoteCurrency, (takerSizePriceHeldSum - takerSizePriceSum) * spec.QuoteScaleK);
            taker.AddToValue(spec.BaseCurrency, takerSizeForThisHandler * spec.BaseScaleK);
        }

        if (takerSizeForThisHandler != 0 || makerSizeForThisHandler != 0)
        {
            FeeCalculationService.AddFeeValue(quoteCurrency, (spec.TakerFee * takerSizeForThisHandler) + (spec.MakerFee * makerSizeForThisHandler));
        }
    }

    private OrderAction GetOppositeAction(OrderAction action)
        => action == OrderAction.ASK ? OrderAction.BID : OrderAction.ASK;
}
