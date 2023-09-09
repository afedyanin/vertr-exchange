using Vertr.Exchange.Common;
using Vertr.Exchange.Infrastructure.EventHandlers;

namespace Vertr.Exchange.Infrastructure.Tests;

[TestFixture(Category = "Unit")]
public class ExchangeCoreServiceTests
{
    [Test]
    public void CanStartExchange()
    {
        var handlers = new List<IOrderCommandEventHandler>() { };
        using var service = new ExchangeCoreService(handlers, LoggerStub.CreateConsoleLogger<ExchangeCoreService>());
        Assert.Pass();
    }

    [Test]
    public void CanProcessOrderSimpleFlow()
    {
        var loggingProcessor = new LoggingProcessor(LoggerStub.CreateConsoleLogger<LoggingProcessor>());

        var handlers = new List<IOrderCommandEventHandler>()
        {
            loggingProcessor
        };

        using var exchange = new ExchangeCoreService(handlers, LoggerStub.CreateConsoleLogger<ExchangeCoreService>());

        var cmd = new OrderCommand
        {
            OrderId = 1,
            OrderType = Common.Enums.OrderType.GTC,
            Command = Common.Enums.OrderCommandType.NOP,
            Price = 1,
        };

        exchange.Send(cmd);

        Assert.Pass();
    }
}
