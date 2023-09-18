using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Infrastructure;
using Vertr.Exchange.Infrastructure.EventHandlers;
using Vertr.Exchange.Tests.Stubs;

namespace Vertr.Exchange.Api.Tests.Stubs;

internal static class ExcnageApiStub
{
    private static IRequestAwaitingService AwaitingService { get; } = new RequestAwatingService(LoggerStub.CreateConsoleLogger<RequestAwatingService>());

    public static IExchangeApi GetNoEnginesApi()
    {
        var api = new ExchangeApi(AwaitingService, NoEnginesExchange);
        return api;
    }

    private static IExchangeCoreService NoEnginesExchange
    {
        get
        {
            var loggingProcessor = new LoggingProcessor(LoggerStub.CreateConsoleLogger<LoggingProcessor>());

            var handlers = new List<IOrderCommandEventHandler>()
            {
                loggingProcessor
            };

            return GetExchangeCoreService(handlers);
        }
    }

    private static IExchangeCoreService GetExchangeCoreService(IEnumerable<IOrderCommandEventHandler> handlers)
    {
        var exchange = new ExchangeCoreService(handlers, LoggerStub.CreateConsoleLogger<ExchangeCoreService>());
        return exchange;
    }
}
