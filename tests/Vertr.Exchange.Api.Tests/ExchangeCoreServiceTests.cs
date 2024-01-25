using Vertr.Exchange.Application.Core;
using Vertr.Exchange.Application.EventHandlers;
using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Application.Tests;

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

        var cmd = new NopCommand();

        exchange.Send(cmd);

        Assert.Pass();
    }

    private sealed class NopCommand : IApiCommand
    {
        public long OrderId => 1L;

        public DateTime Timestamp => DateTime.UtcNow;

        public void Fill(ref OrderCommand command)
        {
            command.Command = Shared.Enums.OrderCommandType.NOP;
            command.OrderId = OrderId;
            command.Timestamp = Timestamp;
        }
    }
}
