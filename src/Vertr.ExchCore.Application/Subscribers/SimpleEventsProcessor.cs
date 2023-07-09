using Microsoft.Extensions.Logging;
using Vertr.ExchCore.Application.Subscribers.Enums;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Abstractions.EventHandlers;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Application.Subscribers;

internal class SimpleEventsProcessor : IOrderCommandSubscriber
{
    private readonly IOrderCommandResultHandler _orderCommandResultHandler;
    private readonly ITradeEventsHandler _tradeEventsHandler;
    private readonly IMarketDataEventsHandler _marketDataEventsHandler;
    private readonly ILogger<SimpleEventsProcessor> _logger;

    public SimpleEventsProcessor(
        IOrderCommandResultHandler orderCommandResultHandler,
        ITradeEventsHandler tradeEventsHandler,
        IMarketDataEventsHandler marketDataEventsHandler,
        ILogger<SimpleEventsProcessor> logger)
    {
        _orderCommandResultHandler = orderCommandResultHandler;
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
        var commandResult = new OrderCommandResult(data, Domain.Enums.CommandResultCode.ACCEPTED, sequence);
        _orderCommandResultHandler.HandleCommandResult(commandResult);
    }

    private void SendTradeEvents(OrderCommand data)
    {
    }

    private void SendMarketData(OrderCommand data)
    {
    }
}
