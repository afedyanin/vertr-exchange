using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Abstractions.EventHandlers;
using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.Events.OrderEvents;
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
    }

    private void SendMarketData(OrderCommand data)
    {
    }
}
