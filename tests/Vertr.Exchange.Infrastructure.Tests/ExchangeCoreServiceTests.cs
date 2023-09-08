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
        var awatingService = new RequestAwatingService(LoggerStub.CreateConsoleLogger<RequestAwatingService>());
        using var service = new ExchangeCoreService(handlers, awatingService, LoggerStub.CreateConsoleLogger<ExchangeCoreService>());
        Assert.Pass();
    }

    [Test]
    public async Task CanProcessOrderSimpleFlow()
    {
        var requestAwatingService = new RequestAwatingService(LoggerStub.CreateConsoleLogger<RequestAwatingService>());
        var loggingProcessor = new LoggingProcessor(LoggerStub.CreateConsoleLogger<LoggingProcessor>());
        var requestCompletetionProcessor = new RequestCompletionProcessor(requestAwatingService, LoggerStub.CreateConsoleLogger<RequestCompletionProcessor>());

        var handlers = new List<IOrderCommandEventHandler>()
        {
            requestCompletetionProcessor, loggingProcessor
        };

        using var exchange = new ExchangeCoreService(handlers, requestAwatingService, LoggerStub.CreateConsoleLogger<ExchangeCoreService>());

        var cmd = new OrderCommand
        {
            OrderId = 1,
            OrderType = Common.Enums.OrderType.GTC,
            Command = Common.Enums.OrderCommandType.NOP,
            Price = 1,
        };

        var cts = new CancellationTokenSource(20000);
        var res = await exchange.Process(cmd, cts.Token);

        Assert.That(res.OrderId, Is.EqualTo(cmd.OrderId));
    }
}
