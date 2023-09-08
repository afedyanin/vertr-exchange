using Microsoft.Extensions.Logging;
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
        using var exchange = CreateExchangeService(handlers);
        Assert.Pass();
    }

    [Test]
    public async Task CanProcessOrderSimpleFlow()
    {
        var requestAwatingService = new RequestAwatingService();
        var logger = CreateConsoleLogger<LoggingProcessor>();
        var loggingProcessor = new LoggingProcessor(logger);
        var requestCompletetionProcessor = new RequestCompletionProcessor(requestAwatingService);

        var handlers = new List<IOrderCommandEventHandler>()
        {
            requestCompletetionProcessor, loggingProcessor
        };

        using var exchange = CreateExchangeService(handlers);

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



    private static IExchangeCoreService CreateExchangeService(IEnumerable<IOrderCommandEventHandler> handlers)
    {
        var awatingService = new RequestAwatingService();
        var logger = CreateConsoleLogger<ExchangeCoreService>();
        var service = new ExchangeCoreService(handlers, awatingService, logger);

        return service;
    }

    private static ILogger<T> CreateConsoleLogger<T>()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddConsole();
        });

        var logger = loggerFactory.CreateLogger<T>();

        return logger;
    }
}
