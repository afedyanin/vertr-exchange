using Vertr.Exchange.Messages;

namespace Vertr.Exchange.Grpc.MessageHandlers;

public class LoggingMessageHandler : IMessageHandler
{
    private readonly ILogger<LoggingMessageHandler> _logger;

    public LoggingMessageHandler(ILogger<LoggingMessageHandler> logger)
    {
        _logger = logger;
    }

    public void CommandResult(ApiCommandResult apiCommandResult)
    {
        _logger.LogInformation($"CommandResult: OrderId={apiCommandResult.OrderId} ResultCode={apiCommandResult.ResultCode}");
    }

    public void OrderBook(OrderBook orderBook)
    {
        _logger.LogInformation($"OrderBook: Symbol={orderBook.Symbol}");
    }

    public void ReduceEvent(ReduceEvent reduceEvent)
    {
        _logger.LogInformation($"ReduceEvent: OrderId={reduceEvent.OrderId} ReducedVolume={reduceEvent.ReducedVolume}");
    }

    public void RejectEvent(RejectEvent rejectEvent)
    {
        _logger.LogInformation($"RejectEvent: OrderId={rejectEvent.OrderId} RejectedVolume={rejectEvent.RejectedVolume}");
    }

    public void TradeEvent(TradeEvent tradeEvent)
    {
        _logger.LogInformation($"TradeEvent: OrderId={tradeEvent.TakerOrderId} TakeOrderCompleted={tradeEvent.TakeOrderCompleted}");
    }
}
