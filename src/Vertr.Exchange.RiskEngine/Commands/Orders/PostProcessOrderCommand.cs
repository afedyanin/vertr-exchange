using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Commands.Orders;

internal class PostProcessOrderCommand
{
    private readonly OrderCommand _orderCommand;
    private readonly IUserProfileService _userProfileService;
    private readonly ISymbolSpecificationProvider _symbolSpecificationProvider;
    private readonly IDictionary<int, LastPriceCacheRecord> _lastPriceCache;
    private readonly bool _marginTradingEnabled;

    public PostProcessOrderCommand(
        IUserProfileService userProfileService,
        ISymbolSpecificationProvider symbolSpecificationProvider,
        IDictionary<int, LastPriceCacheRecord> lastPriceCache,
        OrderCommand command,
        bool marginTradingEnabled)
    {
        _orderCommand = command;
        _userProfileService = userProfileService;
        _symbolSpecificationProvider = symbolSpecificationProvider;
        _lastPriceCache = lastPriceCache;
        _marginTradingEnabled = marginTradingEnabled;
    }

    public bool Execute()
    {
        var symbol = _orderCommand.Symbol;
        var marketData = _orderCommand.MarketData;
        var mte = _orderCommand.MatcherEvent;

        // skip events processing if no events (or if contains BINARY EVENT)
        if (marketData == null && (mte == null || mte.EventType == MatcherEventType.BINARY_EVENT))
        {
            return false;
        }

        var spec = _symbolSpecificationProvider.GetSymbolSpecification(symbol) ??
            throw new InvalidOperationException("Symbol not found: " + symbol);

        var takerSell = _orderCommand.Action == OrderAction.ASK;

        if (mte != null && mte.EventType != MatcherEventType.BINARY_EVENT)
        {
            // at least one event to process, resolving primary/taker user profile
            // TODO processing order is reversed
            if (spec.Type == SymbolType.CURRENCY_EXCHANGE_PAIR)
            {
                var takerUp = _userProfileService.GetUserProfileOrAddSuspended(_orderCommand.Uid);

                // REJECT always comes first; REDUCE is always single event
                if (mte.EventType is MatcherEventType.REDUCE or MatcherEventType.REJECT)
                {
                    if (takerUp != null)
                    {
                        //handleMatcherRejectReduceEventExchange(cmd, mte, spec, takerSell, takerUp);
                    }
                    mte = mte.NextEvent;
                }

                if (mte != null)
                {
                    if (takerSell)
                    {
                        //handleMatcherEventsExchangeSell(mte, spec, takerUp);
                    }
                    else
                    {
                        //handleMatcherEventsExchangeBuy(mte, spec, takerUp, cmd);
                    }
                }
            }
            else
            {

                var takerUp = _userProfileService.GetUserProfileOrAddSuspended(_orderCommand.Uid);

                // for margin-mode symbols also resolve position record
                //var takerSpr = takerUp?.getPositionRecordOrThrowEx(symbol);
                do
                {
                    //handleMatcherEventMargin(mte, spec, cmd.Action, takerUp, takerSpr);
                    mte = mte.NextEvent;
                } while (mte != null);
            }
        }

        // Process marked data
        if (marketData is not null && _marginTradingEnabled)
        {
            // TODO: Check sorting
            var askPrice = (marketData.AskSize != 0) ? marketData.AskPrices[0] : long.MaxValue;
            var bidPrice = (marketData.BidSize != 0) ? marketData.BidPrices[0] : 0;

            if (_lastPriceCache.ContainsKey(symbol))
            {
                _lastPriceCache[symbol] = new LastPriceCacheRecord(askPrice, bidPrice);
            }
            else
            {
                _lastPriceCache.Add(symbol, new LastPriceCacheRecord(askPrice, bidPrice));
            }
        }

        return false;
    }
}
