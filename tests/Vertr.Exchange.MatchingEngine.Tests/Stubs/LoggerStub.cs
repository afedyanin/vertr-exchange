using Microsoft.Extensions.Logging;

namespace Vertr.Exchange.MatchingEngine.Tests.Stubs;
internal static class LoggerStub
{
    private static readonly ILoggerFactory _loggerFactory =
        LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddConsole();
        });

    public static ILogger<T> CreateConsoleLogger<T>()
        => _loggerFactory.CreateLogger<T>();
}
