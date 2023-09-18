using Vertr.Exchange.Api.Awaiting;
using Vertr.Exchange.Api.EventHandlers;
using Vertr.Exchange.Infrastructure;
using Vertr.Exchange.Infrastructure.EventHandlers;
using Vertr.Exchange.Tests.Stubs;

namespace Vertr.Exchange.Api.Tests.Stubs;

internal static class ExcnageApiStub
{
    private static readonly IRequestAwaitingService _awaitingService =
        new RequestAwatingService(LoggerStub.CreateConsoleLogger<RequestAwatingService>());

    private static readonly LoggingProcessor _loggingProcessor =
        new LoggingProcessor(LoggerStub.CreateConsoleLogger<LoggingProcessor>());

    private static readonly RequestCompletionProcessor _completionProcessor =
        new RequestCompletionProcessor(
                _awaitingService,
                LoggerStub.CreateConsoleLogger<RequestCompletionProcessor>());

    public static IExchangeApi GetNoEnginesApi()
    {
        var api = new ExchangeApi(_awaitingService, NoEnginesExchange);
        return api;
    }

    private static IExchangeCoreService NoEnginesExchange
    {
        get
        {
            var handlers = new List<IOrderCommandEventHandler>()
            {
                _loggingProcessor,
                _completionProcessor,
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
