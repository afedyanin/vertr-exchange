using Microsoft.Extensions.Logging;
using Vertr.Exchange.Api.Factories;
using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Core.EventHandlers;


namespace Vertr.Exchange.Api.EventHandlers;

internal class SimpleMessageProcessor : IOrderCommandEventHandler
{
    private readonly IMessageHandler _messageHandler;
    private readonly ILogger<SimpleMessageProcessor> _logger;

    public int ProcessingStep => 1010;

    public SimpleMessageProcessor(
        IMessageHandler messageHandler,
        ILogger<SimpleMessageProcessor> logger)
    {
        _messageHandler = messageHandler;
        _logger = logger;
    }

    public void OnEvent(OrderCommand data, long sequence, bool endOfBatch)
    {
        try
        {
            SendCommandResult(data, sequence);
            SendTradeEvents(data, sequence);
            SendMarketData(data, sequence);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void SendCommandResult(OrderCommand data, long sequence)
    {
        var cmdRes = MessageFactory.CreateApiCommandResult(data, sequence);
        _messageHandler.CommandResult(cmdRes);
    }

    private void SendTradeEvents(OrderCommand data, long sequence)
    {
        var trades = new List<IEngineEvent>();
        var current = data.EngineEvent;

        while (current != null)
        {
            if (current.EventType == EngineEventType.REJECT)
            {
                var reject = MessageFactory.CreateRejectEvent(data, current, sequence);
                _messageHandler.RejectEvent(reject);
            }
            if (current.EventType == EngineEventType.REDUCE)
            {
                var reduce = MessageFactory.CreateReduceEvent(data, current, sequence);
                _messageHandler.ReduceEvent(reduce);
            }
            if (current.EventType == EngineEventType.TRADE)
            {
                trades.Add(current);
            }

            current = current.NextEvent;
        }

        if (trades.Any())
        {
            var tradeEvent = MessageFactory.CreateTradeEvent(data, trades, sequence);
            _messageHandler.TradeEvent(tradeEvent);
        }
    }

    private void SendMarketData(OrderCommand data, long sequence)
    {
        if (data.MarketData == null)
        {
            return;
        }

        var orderBook = MessageFactory.CreateOrderBook(data, data.MarketData, sequence);
        _messageHandler.OrderBook(orderBook);
    }
}
