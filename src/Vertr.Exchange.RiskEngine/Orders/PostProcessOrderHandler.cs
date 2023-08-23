using System.Diagnostics;
using Vertr.Exchange.Accounts.Abstractions;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Symbols;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine.Orders;

internal class PostProcessOrderHandler
{
    private readonly IUserProfilesRepository _userProfiles;
    private readonly ISymbolSpecificationProvider _symbols;

    public PostProcessOrderHandler(
        IUserProfilesRepository userProfiles,
        ISymbolSpecificationProvider symbols)
    {
        _userProfiles = userProfiles;
        _symbols = symbols;
    }

    public bool Handle(OrderCommand orderCommand)
    {
        var matcherEvent = orderCommand.MatcherEvent;

        // skip events processing if no events (or if contains BINARY EVENT)
        if (matcherEvent == null || matcherEvent.EventType == MatcherEventType.BINARY_EVENT)
        {
            return false; // ??
        }

        var spec = _symbols.GetSymbolSpecification(orderCommand.Symbol);
        Debug.Assert(spec != null);

        var takerAction = orderCommand.Action!.Value;
        var takerProfile = _userProfiles.GetOrAdd(orderCommand.Uid, Accounts.Enums.UserStatus.SUSPENDED);

        do
        {
            HandleMatcherEvent(matcherEvent, spec, takerAction, takerProfile);
            matcherEvent = matcherEvent.NextEvent;
        } while (matcherEvent != null);

        return false; // ??
    }

    private void HandleMatcherEvent(
        IMatcherTradeEvent tradeEvent,
        CoreSymbolSpecification spec,
        OrderAction takerAction,
        IUserProfile takerProfile)
    {
        if (tradeEvent.EventType != MatcherEventType.TRADE)
        {
            return;
        }

        // update taker's position
        takerProfile.UpdatePosition(
            spec.SymbolId,
            takerAction,
            tradeEvent.Size,
            tradeEvent.Price,
            spec.QuoteCurrency);

        // update maker's position
        var makerProfile = _userProfiles.GetOrAdd(tradeEvent.MatchedOrderUid, Accounts.Enums.UserStatus.SUSPENDED);

        makerProfile.UpdatePosition(
            spec.SymbolId,
            GetOppositeAction(takerAction),
            tradeEvent.Size,
            tradeEvent.Price,
            spec.QuoteCurrency);
    }

    private OrderAction GetOppositeAction(OrderAction action)
        => action == OrderAction.ASK ? OrderAction.BID : OrderAction.ASK;
}
