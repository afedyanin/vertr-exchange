using System.Diagnostics;
using Vertr.Exchange.Shared.Enums;
using Vertr.Exchange.Domain.RiskEngine.Symbols;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.Common;

namespace Vertr.Exchange.Domain.RiskEngine.Orders;

internal class PostProcessOrderHandler(
    IUserProfileProvider userProfiles,
    ISymbolSpecificationProvider symbols)
{
    private readonly IUserProfileProvider _userProfiles = userProfiles;
    private readonly ISymbolSpecificationProvider _symbols = symbols;

    public void Handle(OrderCommand orderCommand)
    {
        var matcherEvent = orderCommand.EngineEvent;

        // skip events processing if no events (or if contains BINARY EVENT)
        if (matcherEvent == null || matcherEvent.EventType == EngineEventType.BINARY_EVENT)
        {
            return;
        }

        var spec = _symbols.GetSymbol(orderCommand.Symbol);
        Debug.Assert(spec != null);

        var takerAction = orderCommand.Action!.Value;
        var takerProfile = _userProfiles.GetOrAdd(orderCommand.Uid, UserStatus.SUSPENDED);

        do
        {
            HandleMatcherEvent(matcherEvent, spec, takerAction, takerProfile);
            matcherEvent = matcherEvent.NextEvent;
        } while (matcherEvent != null);
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
