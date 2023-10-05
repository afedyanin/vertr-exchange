using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Messages;

namespace Vertr.Exchange.Server.MessageHandlers;

public class LogMessageHandler : IMessageHandler
{
    private readonly ILogger<LogMessageHandler> _logger;

    public LogMessageHandler(ILogger<LogMessageHandler> logger)
    {
        _logger = logger;
    }

    public Task CommandResult(ApiCommandResult apiCommandResult)
    {
        _logger.LogInformation($"CommandResult: OrderId={apiCommandResult.OrderId} ResultCode={apiCommandResult.ResultCode}");
        return Task.CompletedTask;
    }

    public Task OrderBook(OrderBook orderBook)
    {
        _logger.LogInformation($"OrderBook: Symbol={orderBook.Symbol}");
        return Task.CompletedTask;
    }

    public Task ReduceEvent(ReduceEvent reduceEvent)
    {
        _logger.LogInformation($"ReduceEvent: OrderId={reduceEvent.OrderId} ReducedVolume={reduceEvent.ReducedVolume}");
        return Task.CompletedTask;
    }

    public Task RejectEvent(RejectEvent rejectEvent)
    {
        _logger.LogInformation($"RejectEvent: OrderId={rejectEvent.OrderId} RejectedVolume={rejectEvent.RejectedVolume}");
        return Task.CompletedTask;
    }

    public Task TradeEvent(TradeEvent tradeEvent)
    {
        _logger.LogInformation($"TradeEvent: OrderId={tradeEvent.TakerOrderId} TakeOrderCompleted={tradeEvent.TakeOrderCompleted}");
        return Task.CompletedTask;
    }
}
