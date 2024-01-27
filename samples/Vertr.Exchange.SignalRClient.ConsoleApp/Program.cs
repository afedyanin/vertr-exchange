using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts.Enums;
using Vertr.Exchange.Contracts.Requests;
using static Vertr.Exchange.SignalRClient.ConsoleApp.StaticContext;

namespace Vertr.Exchange.SignalRClient.ConsoleApp;

// Need to start Vertr.Exchange.Server host
internal sealed class Program
{
    public static async Task Main()
    {
        var host = CreateHost();

        var api = host.Services.GetRequiredService<IExchangeApiClient>();
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        var cts = new CancellationTokenSource();

        var hostTask = host.RunAsync(cts.Token);

        var tradingTask = InitTrading(api, logger)
            .ContinueWith(async _ =>
            {
                await DoTrading(api, logger, cts);
            });

        await Task.WhenAll(hostTask, tradingTask);

        logger.LogDebug("Stop trading...");
    }

    public static async Task InitTrading(IExchangeApiClient api, ILogger<Program> logger)
    {
        logger.LogDebug("Init trading...");

        _ = await api.AddSymbols(
            new AddSymbolsRequest
            {
                Symbols = Symbols.AllSymbolSpecs
            });

        _ = await api.AddAccounts(
            new AddAccountsRequest
            {
                UserAccounts = UserAccounts.All
            });
    }

    public static async Task DoTrading(
        IExchangeApiClient api,
        ILogger<Program> logger,
        CancellationTokenSource cts)
    {
        Console.WriteLine("=============================================================");
        Console.WriteLine("Press <A> to place ASK, press <B> to place BID, <ESC> to exit.");
        Console.WriteLine("=============================================================");

        var doExit = false;

        while (!doExit)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        doExit = true;
                        cts.Cancel();
                        break;
                    case ConsoleKey.A:
                        await PlaceAsk(api, logger);
                        break;
                    case ConsoleKey.B:
                        await PlaceBid(api, logger);
                        break;
                    default:
                        break;
                }
            }

            await Task.Delay(100);
        }
    }

    private static async Task PlaceAsk(IExchangeApiClient api, ILogger<Program> logger)
    {
        var askOrderId = await api.GetNextOrderId();
        var askOrderResult = await api.PlaceOrder(
            new PlaceOrderRequest
            {
                OrderId = askOrderId,
                OrderType = OrderType.GTC,
                Action = OrderAction.ASK,
                UserId = Users.Alice.Id,
                Price = NextRandomPrice(123),
                Size = NextRandomQty(10),
                Symbol = Symbols.MSFT.Id
            });

        logger.LogWarning("ASK order result: {orderResult}", askOrderResult);
    }

    private static async Task PlaceBid(IExchangeApiClient api, ILogger<Program> logger)
    {
        var bidOrderId = await api.GetNextOrderId();
        var bidOrderResult = await api.PlaceOrder(
            new PlaceOrderRequest
            {
                OrderId = bidOrderId,
                OrderType = OrderType.GTC,
                Action = OrderAction.BID,
                UserId = Users.Bob.Id,
                Price = NextRandomPrice(123),
                Size = NextRandomQty(10),
                Symbol = Symbols.MSFT.Id
            });

        logger.LogWarning("BID order result: {orderResult}", bidOrderResult);
    }

    private static IHost CreateHost()
    {
        var host = new HostBuilder()
              .ConfigureServices((hostContext, services) =>
              {
                  var config = GetConfiguration();
                  services.AddExchangeApi(config);
                  services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
                  services.AddLogging(configure => configure.AddConsole())
                      .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
              })
             .UseConsoleLifetime()
             .Build();

        return host;
    }

    private static IConfiguration GetConfiguration()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        return config;
    }
    private static decimal NextRandomPrice(decimal baseParice)
    {
        var delta = RandomSign() * Random.Shared.Next(100) / 100.0m;
        return baseParice + delta;
    }

    private static int NextRandomQty(int maxValue = 10)
        => Random.Shared.Next(1, maxValue);

    private static int RandomSign()
        => Random.Shared.NextDouble() >= .51 ? -1 : 1;
}
