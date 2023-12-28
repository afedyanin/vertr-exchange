using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Messages;

namespace Vertr.Exchange.Api.Tests.Stubs;

public class MessageHandlerStub : IMessageHandler
{
    private readonly Dictionary<long, ApiCommandResult> _commandResults = [];
    private readonly Dictionary<long, TradeEvent> _tradeEvents = [];
    private readonly Dictionary<long, ReduceEvent> _reduceEvents = [];
    private readonly Dictionary<long, RejectEvent> _rejectEvents = [];

    public void CommandResult(ApiCommandResult apiCommandResult)
    {
        if (!_commandResults.TryAdd(apiCommandResult.OrderId, apiCommandResult))
        {
            _commandResults[apiCommandResult.OrderId] = apiCommandResult;
        }
    }

    public ApiCommandResult? GetApiCommandResult(long orderId)
    {
        _commandResults.TryGetValue(orderId, out var res);
        return res;
    }

    public TradeEvent? GetTradeEvent(long takerOrderId)
    {
        _tradeEvents.TryGetValue(takerOrderId, out var res);
        return res;
    }

    public ReduceEvent? GetReduceEvent(long orderId)
    {
        _reduceEvents.TryGetValue(orderId, out var res);
        return res;
    }

    public RejectEvent? GetRejectEvent(long orderId)
    {
        _rejectEvents.TryGetValue(orderId, out var res);
        return res;
    }

    public void OrderBook(OrderBook orderBook)
    {
    }

    public void ReduceEvent(ReduceEvent reduceEvent)
    {
        if (!_reduceEvents.TryAdd(reduceEvent.OrderId, reduceEvent))
        {
            _reduceEvents[reduceEvent.OrderId] = reduceEvent;
        }
    }

    public void RejectEvent(RejectEvent rejectEvent)
    {
        if (!_rejectEvents.TryAdd(rejectEvent.OrderId, rejectEvent))
        {
            _rejectEvents[rejectEvent.OrderId] = rejectEvent;
        }
    }

    public void TradeEvent(TradeEvent tradeEvent)
    {
        if (!_tradeEvents.TryAdd(tradeEvent.TakerOrderId, tradeEvent))
        {
            _tradeEvents[tradeEvent.TakerOrderId] = tradeEvent;
        }
    }
}
