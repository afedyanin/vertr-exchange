using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Abstractions.EventHandlers;
using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.Events.MarketDataEvents;
using Vertr.ExchCore.Domain.Events.OrderEvents;
using Vertr.ExchCore.Domain.Events.TradeEvents;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Application.Subscribers;

internal class SimpleEventsProcessor : IOrderCommandSubscriber
{
    private readonly IOrderCommandEventHandler _orderCommandEventHandler;
    private readonly ITradeEventHandler _tradeEventsHandler;
    private readonly IMarketDataEventHandler _marketDataEventsHandler;
    private readonly ILogger<SimpleEventsProcessor> _logger;

    public SimpleEventsProcessor(
        IOrderCommandEventHandler orderCommandEventHandler,
        ITradeEventHandler tradeEventsHandler,
        IMarketDataEventHandler marketDataEventsHandler,
        ILogger<SimpleEventsProcessor> logger)
    {
        _orderCommandEventHandler = orderCommandEventHandler;
        _tradeEventsHandler = tradeEventsHandler;
        _marketDataEventsHandler = marketDataEventsHandler;
        _logger = logger;
    }
    public int Priority => (int)GroupPriority.PostProcessing;

    public void HandleEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        _logger.LogDebug("Start Processing OrderId={OrderId}", data.OrderId);

        try
        {
            // TODO: Split on separated consumers
            SendCommandResult(data, sequence);
            SendTradeEvents(data);
            SendMarketData(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing OrderId={OrderId}", data.OrderId);
        }
    }

    private void SendCommandResult(OrderCommand data, long sequence)
    {
        // TODO Implement this
        var evt = new OrderCommandEvent();
        _orderCommandEventHandler.Handle(evt);
    }

    private void SendTradeEvents(OrderCommand data)
    {
        var evts = CreateTradeEvents(data);

        Debug.Assert(evts is not null);

        if (evts.Any())
        {
            _tradeEventsHandler.Handle(evts);
        }
    }

    private void SendMarketData(OrderCommand data)
    {
        var orderBook = CreateOrderBook(data);

        if (orderBook is not null)
        {
            _marketDataEventsHandler.Handle(orderBook);
        }
    }

    private OrderBook? CreateOrderBook(OrderCommand data)
    {
        var l2MarketData = data.L2MarketData;

        if (l2MarketData is null)
        {
            return null;
        }

        var asks = new List<OrderBookRecord>(l2MarketData.AskSize);

        for (int i = 0; i < l2MarketData.AskSize; i++)
        {
            // TODO: Implement this
            asks.Add(new OrderBookRecord());
        }

        // TODO: Implement this
        return new OrderBook();
    }

    private IEnumerable<TradeEventBase> CreateTradeEvents(OrderCommand data)
    {
        var matcherEvt = data.MatcherEvent;
        var res = new List<TradeEventBase>();

        while(matcherEvt is not null)
        {
            var evt = CreateTradeEvent(data, matcherEvt);

            if (evt is not null)
            {
                res.Add(evt);
            }

            matcherEvt = matcherEvt.NextEvent;
        }

        return res;
    }

    private TradeEventBase? CreateTradeEvent(OrderCommand data, MatcherTradeEvent mathcerEvt)
    {
        // TODO Implement this
        return new TradeEvent();
    }
}
