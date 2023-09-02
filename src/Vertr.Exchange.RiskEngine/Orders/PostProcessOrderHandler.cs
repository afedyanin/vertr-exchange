using System.Diagnostics;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.RiskEngine.Symbols;

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
        var matcherEvent = orderCommand.EngineEvent;

        // skip events processing if no events (or if contains BINARY EVENT)
        if (matcherEvent == null || matcherEvent.EventType == EngineEventType.BINARY_EVENT)
        {
            return false; // ??
        }

        var spec = _symbols.GetSymbolSpecification(orderCommand.Symbol);
        Debug.Assert(spec != null);

        var takerAction = orderCommand.Action!.Value;
        var takerProfile = _userProfiles.GetOrAdd(orderCommand.Uid, UserStatus.SUSPENDED);

        do
        {
            HandleMatcherEvent(matcherEvent, spec, takerAction, takerProfile);
            matcherEvent = matcherEvent.NextEvent;
        } while (matcherEvent != null);

        return false; // ??
    }

    private void HandleMatcherEvent(
        IEngineEvent tradeEvent,
        SymbolSpecification spec,
        OrderAction takerAction,
        IUserProfile takerProfile)
    {
        if (tradeEvent.EventType != EngineEventType.TRADE)
        {
            return;
        }

        // update taker's position
        takerProfile.UpdatePosition(spec, takerAction, tradeEvent.Size, tradeEvent.Price);

        // update maker's position
        var makerProfile = _userProfiles.GetOrAdd(tradeEvent.MatchedOrderUid, UserStatus.SUSPENDED);
        makerProfile.UpdatePosition(spec, GetOppositeAction(takerAction), tradeEvent.Size, tradeEvent.Price);
    }

    private OrderAction GetOppositeAction(OrderAction action)
        => action == OrderAction.ASK ? OrderAction.BID : OrderAction.ASK;
}
