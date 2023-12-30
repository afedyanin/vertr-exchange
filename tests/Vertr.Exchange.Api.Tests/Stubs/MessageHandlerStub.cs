using System.Collections.Concurrent;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Messages;

namespace Vertr.Exchange.Api.Tests.Stubs;

public class MessageHandlerStub : IMessageHandler
{
    private readonly ConcurrentDictionary<long, ApiCommandResult> _commandResults = [];
    private readonly ConcurrentDictionary<long, TradeEvent> _tradeEvents = [];
    private readonly ConcurrentDictionary<long, ReduceEvent> _reduceEvents = [];
    private readonly ConcurrentDictionary<long, RejectEvent> _rejectEvents = [];

    public void CommandResult(ApiCommandResult apiCommandResult)
    {
        if (!_commandResults.TryAdd(apiCommandResult.OrderId, apiCommandResult))
        {
            _commandResults[apiCommandResult.OrderId] = apiCommandResult;
        }
    }

    public async Task<ApiCommandResult> GetApiCommandResult(
        long orderId,
        CancellationToken cancellationToken)
        => await GetValueSafe(_commandResults, orderId, cancellationToken);

    public async Task<TradeEvent> GetTradeEvent(
        long takerOrderId,
        CancellationToken cancellationToken)
        => await GetValueSafe(_tradeEvents, takerOrderId, cancellationToken);

    public async Task<ReduceEvent> GetReduceEvent(
        long orderId,
        CancellationToken cancellationToken)
        => await GetValueSafe(_reduceEvents, orderId, cancellationToken);

    public async Task<RejectEvent> GetRejectEvent(
        long orderId,
        CancellationToken cancellationToken)
        => await GetValueSafe(_rejectEvents, orderId, cancellationToken);

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

    private async Task<T> GetValueSafe<T>(
        ConcurrentDictionary<long, T> dict,
        long key, CancellationToken
        cancellationToken = default)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (dict.TryGetValue(key, out var res))
            {
                return res;
            }

            await Task.Delay(10, cancellationToken);
        }
    }
}
