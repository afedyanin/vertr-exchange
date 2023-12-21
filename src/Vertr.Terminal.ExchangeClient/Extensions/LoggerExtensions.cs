using Microsoft.Extensions.Logging;

namespace Vertr.Terminal.ExchangeClient.Extensions;

// https://darchuk.net/2019/10/18/logging-in-a-net-core-3-signalr-client/
internal static class LoggerExtensions
{
    public static ILoggerProvider AsLoggerProvider(this ILogger logger)
    {
        return new ExistingLoggerProvider(logger);
    }

    private class ExistingLoggerProvider(ILogger logger) : ILoggerProvider
    {
        private readonly ILogger _logger = logger;

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
        }
    }
}
