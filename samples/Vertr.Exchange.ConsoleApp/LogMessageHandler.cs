using Microsoft.Extensions.Logging;
using Vertr.Exchange.Application.Messages;

namespace Vertr.Exchange.ConsoleApp;
internal sealed class LogMessageHandler(ILogger<LogMessageHandler> logger) : IMessageHandler
{
    private readonly ILogger<LogMessageHandler> _logger = logger;

    public Guid Id { get; } = Guid.NewGuid();

    public void CommandResult(ApiCommandResult apiCommandResult)
    {
        _logger.LogInformation("ApiCommandResult received. {CommandResult}", apiCommandResult);
    }

    public void OrderBook(OrderBook orderBook)
    {
        _logger.LogInformation("OrderBook received. {orderBook}", orderBook);
    }

    public void ReduceEvent(ReduceEvent reduceEvent)
    {
        _logger.LogInformation("ReduceEvent received. {reduceEvent}", reduceEvent);
    }

    public void RejectEvent(RejectEvent rejectEvent)
    {
        _logger.LogInformation("RejectEvent received. {rejectEvent}", rejectEvent);
    }

    public void TradeEvent(TradeEvent tradeEvent)
    {
        _logger.LogInformation("TradeEvent received. {tradeEvent}", tradeEvent);
    }
}
