using System.Collections.Concurrent;
using Vertr.Exchange.Application.Messages;

namespace Vertr.Exchange.Application.Tests.Stubs;

public class MessageHandlerStub : IMessageHandler
{
    private readonly ConcurrentDictionary<long, ApiCommandResult> _commandResults = [];
    private readonly ConcurrentDictionary<long, TradeEvent> _tradeEvents = [];
    private readonly ConcurrentDictionary<long, ReduceEvent> _reduceEvents = [];
    private readonly ConcurrentDictionary<long, RejectEvent> _rejectEvents = [];
    private readonly ConcurrentDictionary<long, OrderBook> _orderBooks = [];

    public Guid Id { get; } = Guid.NewGuid();

    public void CommandResult(ApiCommandResult apiCommandResult)
    {
        _commandResults.AddOrUpdate(
            apiCommandResult.OrderId,
            apiCommandResult,
            (key, oldVal) => { return apiCommandResult; });
    }

    public void Reset()
    {
        _commandResults.Clear();
        _tradeEvents.Clear();
        _reduceEvents.Clear();
        _rejectEvents.Clear();
        _orderBooks.Clear();
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

    public async Task<OrderBook> GetOrderBook(
        long symbol,
        CancellationToken cancellationToken)
        => await GetValueSafe(_orderBooks, symbol, cancellationToken);

    public void OrderBook(OrderBook orderBook)
    {
        _orderBooks.AddOrUpdate(
            orderBook.Symbol,
            orderBook,
            (key, oldVal) => { return orderBook; });
    }

    public void ReduceEvent(ReduceEvent reduceEvent)
    {
        _reduceEvents.AddOrUpdate(
            reduceEvent.OrderId,
            reduceEvent,
            (key, oldVal) => { return reduceEvent; });
    }

    public void RejectEvent(RejectEvent rejectEvent)
    {
        _rejectEvents.AddOrUpdate(
            rejectEvent.OrderId,
            rejectEvent,
            (key, oldVal) => { return rejectEvent; });
    }

    public void TradeEvent(TradeEvent tradeEvent)
    {
        _tradeEvents.AddOrUpdate(
            tradeEvent.TakerOrderId,
            tradeEvent,
            (key, oldVal) => { return tradeEvent; });
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
