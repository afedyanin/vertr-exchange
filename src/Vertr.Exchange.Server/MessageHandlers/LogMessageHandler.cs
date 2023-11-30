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
